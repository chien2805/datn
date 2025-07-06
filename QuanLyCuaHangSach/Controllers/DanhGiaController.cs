using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;

namespace QuanLyCuaHangSach.Controllers
{
    public class DanhGiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhGiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemDanhGia(DanhGia model)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");

            if (maTaiKhoan == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đánh giá.";
                return RedirectToAction("Login", "TaiKhoan");
            }

            bool daTonTai = _context.DanhGias.Any(x => x.MaSach == model.MaSach && x.MaTaiKhoan == maTaiKhoan);
            if (daTonTai)
            {
                TempData["Error"] = "Bạn đã đánh giá sách này.";
                return RedirectToAction("Details", "Sach", new { id = model.MaSach });
            }

            model.MaTaiKhoan = maTaiKhoan.Value;
            model.NgayTao = DateTime.Now;

            _context.DanhGias.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Cảm ơn bạn đã đánh giá!";
            return RedirectToAction("Details", "Sach", new { id = model.MaSach });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaDanhGia(int id, int maSach)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập.";
                return RedirectToAction("Login", "TaiKhoan");
            }

            var danhGia = _context.DanhGias.FirstOrDefault(x => x.Id == id && x.MaTaiKhoan == maTaiKhoan);
            if (danhGia == null)
            {
                TempData["Error"] = "Không tìm thấy hoặc bạn không có quyền xóa bình luận này.";
                return RedirectToAction("Details", "Sach", new { id = maSach });
            }

            _context.DanhGias.Remove(danhGia);
            _context.SaveChanges();
            TempData["Success"] = "Xóa bình luận thành công.";
            return RedirectToAction("Details", "Sach", new { id = maSach });
        }

    }
}
