using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuanLyCuaHangSach.Models.Momo;
using QuanLyCuaHangSach.Models.Order;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangSach.Services
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }
        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = $"Khách hàng: {model.FullName}, Địa chỉ: {model.DiaChi}, SĐT: {model.SoDienThoai}. Nội dung: {model.OrderInfo}";

            // Nếu có danh sách sách, thêm vào extraData
            string extraData = "";
            if (model.DanhSachSach != null && model.DanhSachSach.Any())
            {
                extraData = string.Join("|", model.DanhSachSach.Select(s =>
                    $"{s.MaSach}-{s.TieuDe} ({s.SoLuong} x {((int)s.DonGia)} VND)"
                ));
            }


            var rawData =
                $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.Amount}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&returnUrl={_options.Value.ReturnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData={extraData}";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = extraData, // ✅ Thêm danh sách sách vào extraData
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            var jsonRequest = JsonConvert.SerializeObject(requestData);
            Console.WriteLine($"Request Data: {jsonRequest}"); // Kiểm tra JSON gửi đi

            // ✅ Ghi log gửi và nhận
            var logDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Logs");
            Directory.CreateDirectory(logDir);

            System.IO.File.WriteAllText(Path.Combine(logDir, "momo_request.json"), jsonRequest);
            System.IO.File.WriteAllText(Path.Combine(logDir, "momo_response.json"), response?.Content ?? "Không có nội dung phản hồi");

            if (string.IsNullOrEmpty(response?.Content))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }


        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;

            // ✅ Chuyển đổi extraData từ chuỗi thành danh sách sách
            // ✅ Kiểm tra extraData trước khi gọi Split
            // ✅ Kiểm tra và ép kiểu extraData thành string
            var extraDataObj = collection.FirstOrDefault(s => s.Key == "extraData").Value;
            string extraData = extraDataObj.ToString() ?? ""; // Chắc chắn là string

            List<SachModel> danhSachSach = new List<SachModel>();

            // ✅ Kiểm tra extraData có null hoặc rỗng trước khi dùng Split
            if (!string.IsNullOrWhiteSpace(extraData))
            {
                string[] books = extraData.Split('|', StringSplitOptions.RemoveEmptyEntries); // ✅ Tách từng sách bằng "|"

                foreach (string book in books)
                {
                    // ✅ Cập nhật regex để lấy cả MaSach
                    var match = Regex.Match(book, @"^(\d+)-(.+?) \((\d+) x (\d+) VND\)$");
                    if (match.Success)
                    {
                        danhSachSach.Add(new SachModel
                        {
                            MaSach = int.Parse(match.Groups[1].Value), // ✅ Lấy MaSach
                            TieuDe = match.Groups[2].Value.Trim(),
                            SoLuong = int.Parse(match.Groups[3].Value),
                            DonGia = int.Parse(match.Groups[4].Value)
                        });
                    }
                }
            }


            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                DanhSachSach = danhSachSach // ✅ Trả về danh sách sách nếu có
            };
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }
}
