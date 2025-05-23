using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

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
            else if (user.VaiTro == "NhanVien")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "HoaDonBan") });
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

        [HttpPost]
        public IActionResult SendResetLink(string email)
        {
            var user = _context.TaiKhoanNguoiDung.FirstOrDefault(u => u.TenDangNhap == email);
            if (user == null)
            {
                return Json(new { success = false, message = "Email không tồn tại trong hệ thống." });
            }

            // Tạo token (ví dụ: guid)
            var token = Guid.NewGuid().ToString();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);
            _context.SaveChanges();

            var resetLink = Url.Action("ResetPassword", "DangNhap", new { token = token }, Request.Scheme);
            var subject = "Đặt lại mật khẩu";
            var body = $"Nhấn vào liên kết sau để đặt lại mật khẩu: <a href='{resetLink}'>Đặt lại mật khẩu</a>";

            // Gửi email
            SendEmail(email, subject, body); // Viết hàm này riêng

            return Json(new { success = true });
        }

        public IActionResult ResetPassword(string token)
        {
            var user = _context.TaiKhoanNguoiDung.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
            if (user == null)
            {
                return Content("Liên kết không hợp lệ hoặc đã hết hạn.");
            }

            return View(model: token); // Truyền token vào view
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword)
        {
            var user = _context.TaiKhoanNguoiDung.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
            if (user == null)
            {
                return Content("Liên kết không hợp lệ hoặc đã hết hạn.");
            }

            user.MatKhau = HashPassword(newPassword); // ✅ Hash lại mật khẩu
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            _context.SaveChanges();

            return View("ResetPasswordSuccess"); // ✅ Hiển thị trang sau khi đổi mật khẩu
        }

        public void SendEmail(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("vanchien280502@gmail.com", "jtqe zzua aani ygai"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("vanchien280502@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            smtpClient.Send(mailMessage);
        }
    }
}
