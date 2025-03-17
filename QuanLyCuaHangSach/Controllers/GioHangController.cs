using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Models;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Helpers;
using System.Linq;

namespace QuanLyCuaHangSach.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng dưới dạng JSON
        public IActionResult GetGioHangJson()
        {
            var gioHang = GetGioHang();
            return Json(gioHang);
        }

        private GioHang GetGioHang()
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();
            if (gioHang.ChiTietGioHangs == null)
                gioHang.ChiTietGioHangs = new List<ChiTietGioHang>();

            foreach (var item in gioHang.ChiTietGioHangs)
            {
                if (item.Sach == null)
                {
                    item.Sach = _context.Sach.FirstOrDefault(s => s.MaSach == item.MaSach);
                }
            }

            return gioHang;
        }

        // Hiển thị giỏ hàng
        public IActionResult XemGioHang()
        {
            var gioHang = GetGioHang();
            return Json(gioHang.ChiTietGioHangs);
        }

        // Cập nhật số lượng sách trong giỏ hàng
        [HttpPost]
        public IActionResult CapNhatSoLuong(int maSach, int soLuongThayDoi)
        {
            var gioHang = GetGioHang();
            var item = gioHang.ChiTietGioHangs.FirstOrDefault(c => c.MaSach == maSach);

            if (item != null)
            {
                item.SoLuong += soLuongThayDoi;
                if (item.SoLuong <= 0)
                    gioHang.ChiTietGioHangs.Remove(item);

                HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
            }
            return Json(new { success = true });
        }

        // Xóa một sách khỏi giỏ hàng
        [HttpPost]
        public IActionResult XoaKhoiGio(int maSach)
        {
            var gioHang = GetGioHang();
            gioHang.ChiTietGioHangs.RemoveAll(c => c.MaSach == maSach);
            HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
            return Json(new { success = true });
        }

        // Thêm sách vào giỏ hàng
        [HttpPost]
        public IActionResult ThemVaoGio(int maSach)
        {
            var sach = _context.Sach.Find(maSach);
            if (sach == null)
                return Json(new { success = false });

            var gioHang = GetGioHang();
            var chiTiet = gioHang.ChiTietGioHangs.FirstOrDefault(c => c.MaSach == maSach);

            if (chiTiet == null)
            {
                gioHang.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    MaSach = maSach,
                    SoLuong = 1
                });
            }
            else
            {
                chiTiet.SoLuong++;
            }

            HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
            return Json(new { success = true });
        }

        // Xóa toàn bộ giỏ hàng
        [HttpPost]
        public IActionResult XoaGioHang()
        {
            HttpContext.Session.Remove("GioHang");
            return Json(new { success = true });
        }
    }
}
