using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace QuanLyCuaHangSach.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangNhapController(ApplicationDbContext context)
        {
            _context = context;
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Định dạng hex
                }
                return builder.ToString();
            }
        }

        // Hiển thị form đăng nhập
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.TaiKhoanNguoiDung
                .FirstOrDefault(u => u.TenDangNhap == email);

            if (user == null)
            {
                // Trả về thông báo lỗi dạng JSON
                return Json(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            var hashedPassword = HashPassword(password);

            if (user.MatKhau != hashedPassword)
            {
                // Trả về thông báo lỗi dạng JSON
                return Json(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            // Tiếp tục như cũ nếu đăng nhập thành công...
            var userInfo = _context.ThongTinNguoiDung
                .FirstOrDefault(t => t.MaTaiKhoan == user.MaTaiKhoan);

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

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.TenDangNhap),
        new Claim(ClaimTypes.Role, user.VaiTro),
        new Claim("MaTaiKhoan", user.MaTaiKhoan.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

            HttpContext.Session.SetString("UserEmail", user.TenDangNhap);
            HttpContext.Session.SetString("UserRole", user.VaiTro);
            HttpContext.Session.SetInt32("MaTaiKhoan", user.MaTaiKhoan);
            HttpContext.Session.SetString("HoTen", userInfo.HoTen);
            HttpContext.Session.SetString("AnhDaiDien", userInfo.AnhDaiDien ?? "");
            HttpContext.Session.SetString("SoDienThoai", userInfo.SoDienThoai);
            HttpContext.Session.SetString("DiaChi", userInfo.DiaChi);

            if (user.VaiTro == "Admin")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Admin", "TrangChu") });
            }
            else
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "TrangChu") });
            }
        }


        // Đăng xuất
        public async Task<IActionResult> Logout()
        {
            // Xóa Session
            HttpContext.Session.Clear();

            // Xóa Claims bằng cách SignOut khỏi hệ thống xác thực
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "TrangChu");
        }

    }
}
