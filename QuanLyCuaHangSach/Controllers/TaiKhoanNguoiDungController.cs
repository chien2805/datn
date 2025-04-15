using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Text;
using System.Security.Cryptography;

namespace QuanLyCuaHangSach.Controllers
{
    public class TaiKhoanNguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiKhoanNguoiDungController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // Hiển thị danh sách tất cả tài khoản người dùng
        [HttpGet]
        public IActionResult Index()
        {
            var taiKhoans = _context.TaiKhoanNguoiDung.ToList();
            return View(taiKhoans);
        }

        // Hiển thị form tạo mới tài khoản người dùng
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo mới tài khoản người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaiKhoanNguoiDung model)
        {
            if (!ModelState.IsValid)
            {
                _context.TaiKhoanNguoiDung.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Hiển thị thông tin chi tiết của một tài khoản người dùng
        [HttpGet]
        public IActionResult Details(int id)
        {
            var taiKhoan = _context.TaiKhoanNguoiDung.Find(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }


        // Hiển thị form chỉnh sửa thông tin tài khoản người dùng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var taiKhoan = _context.TaiKhoanNguoiDung.Find(id);
            if (taiKhoan == null) return NotFound();

            // Trả về JSON với PascalCase
            return Json(new
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                TenDangNhap = taiKhoan.TenDangNhap,
                VaiTro = taiKhoan.VaiTro
            });
        }


        // Xử lý cập nhật thông tin tài khoản người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaiKhoanNguoiDung model)
        {
            // Tìm bản ghi gốc
            var taiKhoan = _context.TaiKhoanNguoiDung.Find(model.MaTaiKhoan);
            if (taiKhoan == null)
                return NotFound();

            // Nếu dữ liệu hợp lệ, cập nhật và chuyển về Index
            if (!ModelState.IsValid)
            {
                // Chỉ hash và gán mật khẩu khi người dùng nhập mới
                if (!string.IsNullOrWhiteSpace(model.MatKhau))
                    taiKhoan.MatKhau = HashPassword(model.MatKhau);

                // Cập nhật vai trò
                taiKhoan.VaiTro = model.VaiTro;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, load lại danh sách tài khoản và trả về Index
            var ds = _context.TaiKhoanNguoiDung.ToList();
            return View("Index", ds);
        }


        // Xoá tài khoản người dùng theo ID
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var taiKhoan = _context.TaiKhoanNguoiDung.Find(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoanNguoiDung.Remove(taiKhoan);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
