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
        string jsonPath = Path.Combine(_env.WebRootPath, "chatbox-mrob-457727cec1a7.json");

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
        var intentName = queryResult.Intent.DisplayName;
        var parameters = queryResult.Parameters;
        string reply = queryResult.FulfillmentText.Replace("\\n", "\n");

        switch (intentName)
        {
            case "TimSachTheoTheLoai":
                if (parameters.Fields.TryGetValue("theloai", out var theLoaiField))
                {
                    string tenTheLoai = theLoaiField.StringValue;

                    var sachList = _context.Sach
                        .Where(s => s.TheLoai.TenTheLoai == tenTheLoai)
                        .Select(s => s.TieuDe)
                        .ToList();

                    reply += sachList.Any()
                        ? $"\n📚 Các sách thuộc thể loại *{tenTheLoai}*:\n- {string.Join("\n- ", sachList)}"
                        : $"\n😢 Hiện chưa có sách nào thuộc thể loại *{tenTheLoai}*.";
                }
                break;

            case "TimSachTheoTen":
                if (parameters.Fields.TryGetValue("tieude", out var tenSachField))
                {
                    string tenSach = tenSachField.StringValue;

                    // Lấy tất cả sách có chứa tên tìm kiếm
                    var sach = _context.Sach
                        .Where(s => s.TieuDe.Contains(tenSach))
                        .ToList();

                    if (sach.Count == 1)
                    {
                        // Nếu chỉ có 1 sách
                        var firstBook = sach.First();
                        // Tạo đường dẫn đến trang chi tiết sách
                        string chiTietLink = $"https://localhost:7115/Sach/Details/{firstBook.MaSach}";
                        reply += $"\n📘 {firstBook.TieuDe} - Giá: {firstBook.Gia}vnđ - {(firstBook.SoLuongTon > 0 ? "Còn hàng" : "Hết hàng")}";
                    }
                    else
                    {
                        // Nếu không tìm thấy sách nào
                        reply += $"\n❌ Không tìm thấy sách có tên *{tenSach}*.";
                    }
                }
                break;

            case "KiemTraTonKho":
                if (parameters.Fields.TryGetValue("tieude", out var tenSachTonField))
                {
                    string tenSachTon = tenSachTonField.StringValue;

                    var sachTon = _context.Sach
                        .FirstOrDefault(s => s.TieuDe.Contains(tenSachTon));

                    if (sachTon != null)
                    {
                        reply += sachTon.SoLuongTon > 0
                            ? $"\n✅ Sách *{sachTon.TieuDe}* hiện đang còn hàng ({sachTon.SoLuongTon} cuốn)."
                            : $"\n❌ Rất tiếc, sách *{sachTon.TieuDe}* hiện đã hết hàng.";
                    }
                    else
                    {
                        reply += $"\n❓ Không tìm thấy sách có tên *{tenSachTon}*.";
                    }
                }
                break;

            case "ThongTinLienHe":
                reply += "\n📞 Bạn có thể liên hệ chúng tôi qua số điện thoại: 0123456789.";
                reply += "\n🕒 Giờ làm việc: Từ 8:00 AM đến 6:00 PM (Thứ Hai đến Thứ Bảy).";
                break;

            case "ChinhSachDoiTra":
                reply += "\n📚 Chính sách đổi trả: Chúng tôi chấp nhận đổi trả sách trong vòng 7 ngày kể từ ngày mua, với điều kiện sách còn nguyên vẹn và không bị hư hỏng.";
                break;

            case "ThanhToan":
                reply += "\n💳 Chúng tôi hỗ trợ các hình thức thanh toán sau:";
                reply += "\n- Thanh toán qua Momo";
                reply += "\n- Thanh toán khi nhận hàng";
                break;



                // Có thể mở rộng thêm intent khác tại đây
        }

        return Content(reply, "text/plain", System.Text.Encoding.UTF8);
    }

}
