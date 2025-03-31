using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using QuanLyCuaHangSach.Models.Order;
using QuanLyCuaHangSach.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangSach.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        private readonly ApplicationDbContext _context;
        public PaymentController(IMomoService momoService, ApplicationDbContext context)
        {
            _momoService = momoService;
            _context = context; // Inject DbContext
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            if (response == null || string.IsNullOrEmpty(response.PayUrl))
            {
                return BadRequest("Không nhận được URL thanh toán từ MoMo.");
            }
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);

            if (response != null)
            {
                // 🔹 Lấy thông tin từ response
                var orderInfo = response.OrderInfo;

                var tenKhachHangMatch = Regex.Match(orderInfo, @"Khách hàng: (.*?),");
                var diaChiMatch = Regex.Match(orderInfo, @"Địa chỉ: (.*?),");
                // 🔹 Tìm thông tin theo mẫu
                var soDienThoaiMatch = Regex.Match(orderInfo, @"SĐT:\s*(\d+)");

                var tenKhachHang = tenKhachHangMatch.Success ? tenKhachHangMatch.Groups[1].Value.Trim() : "";
                var diaChi = diaChiMatch.Success ? diaChiMatch.Groups[1].Value.Trim() : "";
                var soDienThoai = soDienThoaiMatch.Success ? soDienThoaiMatch.Groups[1].Value.Trim() : "";

                Console.WriteLine(tenKhachHang);
                Console.WriteLine(diaChi);
                Console.WriteLine(soDienThoai);
                // 🔹 Tạo hóa đơn mới
                var hoaDon = new HoaDonBanOnline
                {
                    TenKhachHang = tenKhachHang,
                    DiaChi = diaChi,
                    SoDienThoai = soDienThoai,
                    NgayTao = DateTime.Now,
                    TongTien = decimal.Parse(response.Amount),
                    TrangThai = "Đã thanh toán", // ✅ Thêm trạng thái đơn hàng
                    LoaiThanhToan = "Momo"
                };
                _context.HoaDonBanOnline.Add(hoaDon);
                _context.SaveChanges(); // Lưu hóa đơn để có MaHoaDon
                Console.WriteLine("Hóa đơn được lưu với MaHoaDon = " + hoaDon.MaHoaDon);

                // 🔹 Lưu danh sách sách vào ChiTietHoaDonOnline (nếu có)
                var extraData = HttpContext.Request.Query["extraData"].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(extraData))
                {
                    var danhSachSach = extraData.Split('|', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in danhSachSach)
                    {
                        var parts = item.Split(new[] { "-", " (" }, StringSplitOptions.None);
                        if (parts.Length < 3) continue;

                        if (!int.TryParse(parts[0].Trim(), out int maSach)) continue;
                        var tenSach = parts[1].Trim();
                        var soLuongGia = parts[2].Replace(")", "").Split('x', StringSplitOptions.RemoveEmptyEntries);
                        if (soLuongGia.Length < 2) continue;

                        if (!int.TryParse(soLuongGia[0].Trim(), out int soLuong)) continue;
                        var donGiaStr = soLuongGia[1].Replace("VND", "").Trim();
                        if (!decimal.TryParse(donGiaStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal donGia)) continue;

                        var chiTiet = new ChiTietHoaDonOnline
                        {
                            MaHoaDon = hoaDon.MaHoaDon,
                            MaSach = maSach, // ✅ Sử dụng MaSach lấy từ extraData
                            TieuDe = tenSach,
                            SoLuong = soLuong,
                            DonGia = donGia,
                            ThanhTien = soLuong * donGia
                        };
                        _context.ChiTietHoaDonOnline.Add(chiTiet);
                        // 🔹 Giảm số lượng sách trong kho
                        var sach = _context.Sach.FirstOrDefault(s => s.MaSach == maSach);
                        if (sach != null)
                        {
                            sach.SoLuongTon -= soLuong;
                            if (sach.SoLuongTon < 0) sach.SoLuongTon = 0; // Đảm bảo không âm
                        }
                    }

                    _context.SaveChanges(); // ✅ Lưu chi tiết hóa đơn sau khi đã có hóa đơn trong DB
                }
                // 🔹 Xóa giỏ hàng sau khi thanh toán thành công
                HttpContext.Session.Remove("GioHang"); // ✅ Xóa giỏ hàng khỏi session
            }

            return View(response);
        }




    }
}
