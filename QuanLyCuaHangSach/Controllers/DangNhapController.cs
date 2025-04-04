using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Security.Claims;

namespace QuanLyCuaHangSach.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangNhapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị form đăng nhập
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Xử lý đăng nhập
        // Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.TaiKhoanNguoiDung
                .FirstOrDefault(u => u.TenDangNhap == email && u.MatKhau == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return View("Index");
            }

            // Lấy thông tin người dùng dựa trên MaTaiKhoan
            var userInfo = _context.ThongTinNguoiDung
                .FirstOrDefault(t => t.MaTaiKhoan == user.MaTaiKhoan);

            // Nếu thông tin người dùng chưa tồn tại, tạo mới thông tin trống
            if (userInfo == null)
            {
                userInfo = new ThongTinNguoiDung
                {
                    MaTaiKhoan = user.MaTaiKhoan,
                    HoTen = "",
                    Email = user.TenDangNhap,
                    SoDienThoai = "",
                    DiaChi = "",
                    AnhDaiDien = ""
                };
                _context.ThongTinNguoiDung.Add(userInfo);
                _context.SaveChanges();
            }
            // 📌 Tạo Claims để lưu thông tin người dùng
            // Tạo danh sách claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.TenDangNhap),
        new Claim(ClaimTypes.Role, user.VaiTro),
        new Claim("MaTaiKhoan", user.MaTaiKhoan.ToString())
    };

            // Tạo danh tính người dùng
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            // Đăng nhập bằng cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Ghi nhớ đăng nhập
            };

            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);
            // Lưu thông tin tài khoản vào session
            HttpContext.Session.SetString("UserEmail", user.TenDangNhap);
            HttpContext.Session.SetString("UserRole", user.VaiTro);
            HttpContext.Session.SetInt32("MaTaiKhoan", user.MaTaiKhoan);

            // Lưu thông tin người dùng vào session
            HttpContext.Session.SetString("HoTen", userInfo.HoTen);
            HttpContext.Session.SetString("AnhDaiDien", userInfo.AnhDaiDien ?? "");
            HttpContext.Session.SetString("SoDienThoai", userInfo.SoDienThoai);
            HttpContext.Session.SetString("DiaChi", userInfo.DiaChi);

            // Kiểm tra vai trò người dùng và chuyển hướng đến trang phù hợp
            if (user.VaiTro == "Admin")
            {
                return RedirectToAction("Index", "Sach"); // Nếu là Admin, chuyển đến trang sách
            }
            else
            {
                return RedirectToAction("Index", "TrangChu"); // Nếu là vai trò khác, chuyển đến trang chủ
            }
        }


        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "TrangChu");
        }
    }
}
