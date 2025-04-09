using Google.Cloud.Dialogflow.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using QuanLyCuaHangSach.Context;
using System;
using System.Linq;

public class ChatbotController : Controller
{
    private readonly IWebHostEnvironment _env;
    private static readonly string projectId = "chatbox-mrob"; // Project ID
    private readonly ApplicationDbContext _context;

    public ChatbotController(IWebHostEnvironment env, ApplicationDbContext context)
    {
        _env = env;
        _context = context;
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetResponse(string userMessage)
    {
        // Đường dẫn đến file credentials JSON
        string jsonPath = Path.Combine(_env.WebRootPath, "chatbox-mrob-b9773ec4f7cf.json");

        var builder = new SessionsClientBuilder
        {
            CredentialsPath = jsonPath
        };
        var client = await builder.BuildAsync();

        var sessionId = Guid.NewGuid().ToString();
        var session = SessionName.FromProjectSession(projectId, sessionId);

        var queryInput = new QueryInput
        {
            Text = new TextInput
            {
                Text = userMessage,
                LanguageCode = "vi"
            }
        };

        var response = await client.DetectIntentAsync(session, queryInput);
        var queryResult = response.QueryResult;
        string reply = queryResult.FulfillmentText;
        reply = reply.Replace("\\n", "\n"); // xử lý xuống dòng đúng
        // ==== Lấy tham số "theloai" từ Dialogflow ====
        var parameters = queryResult.Parameters;
        if (parameters.Fields.TryGetValue("theloai", out var theLoaiField))
        {
            string tenTheLoai = theLoaiField.StringValue;

            // Tìm sách trong CSDL theo thể loại
            var sachList = _context.Sach
                .Where(s => s.TheLoai.TenTheLoai == tenTheLoai)
                .Select(s => s.TieuDe)
                .ToList();

            if (sachList.Any())
            {
                reply += $"\n📚 Các sách thuộc thể loại *{tenTheLoai}*:\n- " + string.Join("\n- ", sachList);
            }
            else
            {
                reply += $"\n😢 Hiện chưa có sách nào thuộc thể loại *{tenTheLoai}*.";
            }
        }

        return Content(reply, "text/plain", System.Text.Encoding.UTF8); // ✅ set UTF-8
    }
}
