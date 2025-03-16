using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;

namespace QuanLyCuaHangSach.Controllers
{
    public class TaiKhoanNguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiKhoanNguoiDungController(ApplicationDbContext context)
        {
            _context = context;
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
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }

        // Xử lý cập nhật thông tin tài khoản người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaiKhoanNguoiDung model)
        {
            if (!ModelState.IsValid)
            {
                _context.TaiKhoanNguoiDung.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
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
