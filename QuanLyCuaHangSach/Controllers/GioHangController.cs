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

            if (item == null)
            {
                return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng." });
            }

            // Lấy thông tin sách từ database để kiểm tra số lượng tồn kho
            var sach = _context.Sach.Find(maSach);
            if (sach == null)
            {
                return Json(new { success = false, message = "Sách không tồn tại." });
            }

            int newQuantity = item.SoLuong + soLuongThayDoi;

            if (newQuantity <= 0)
            {
                // Nếu số lượng mới là 0 hoặc âm, xóa khỏi giỏ hàng
                gioHang.ChiTietGioHangs.Remove(item);
                HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
                return Json(new { success = true, message = "Đã xóa sách khỏi giỏ hàng." });
            }
            else if (newQuantity > sach.SoLuongTon)
            {
                // Nếu số lượng mới vượt quá số lượng tồn kho
                return Json(new { success = false, message = $"Hiện tại số lượng đang còn tại cửa hàng của sách '{sach.TieuDe}' là {sach.SoLuongTon}." });
            }
            else
            {
                // Cập nhật số lượng nếu hợp lệ
                item.SoLuong = newQuantity;
                HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
                return Json(new { success = true, message = "Cập nhật số lượng thành công." });
            }
        }

        [HttpPost]
        public IActionResult CapNhatSoLuongIndex(int maSach, int soLuongThayDoi)
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