using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;

namespace QuanLyCuaHangSach.Controllers
{
    public class ThongTinNguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThongTinNguoiDungController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThongTinNguoiDung
        public async Task<IActionResult> Index()
        {
            var thongTinNguoiDung = await _context.ThongTinNguoiDung.Include(t => t.TaiKhoanNguoiDung).ToListAsync();
            return View(thongTinNguoiDung);
        }

        // GET: ThongTinNguoiDung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var thongTinNguoiDung = await _context.ThongTinNguoiDung
                .Include(t => t.TaiKhoanNguoiDung)
                .FirstOrDefaultAsync(m => m.MaThongTinNguoiDung == id);

            if (thongTinNguoiDung == null)
            {
                thongTinNguoiDung = new ThongTinNguoiDung
                {
                    MaTaiKhoan = id.Value
                };
                _context.ThongTinNguoiDung.Add(thongTinNguoiDung);
                await _context.SaveChangesAsync();
            }

            return View(thongTinNguoiDung);
        }

        // Phương thức chỉnh sửa thông tin người dùng
        [HttpPost]
        public IActionResult Edit(ThongTinNguoiDung thongTin)
        {
            // Kiểm tra mã tài khoản nhận được
            Console.WriteLine($"MaTaiKhoan nhận được: {thongTin.MaTaiKhoan}");

            // Kiểm tra dữ liệu đầu vào có hợp lệ không
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorMessage = string.Join("; ", errors);
                Console.WriteLine($"Lỗi ModelState: {errorMessage}");

                // Trả về lỗi nếu dữ liệu không hợp lệ
                return BadRequest($"Dữ liệu không hợp lệ: {errorMessage}");
            }

            // Tìm thông tin người dùng dựa trên MaTaiKhoan
            var userInfo = _context.ThongTinNguoiDung.FirstOrDefault(t => t.MaTaiKhoan == thongTin.MaTaiKhoan);

            if (userInfo == null)
            {
                Console.WriteLine("Không tìm thấy thông tin người dùng trong database");
                return NotFound("Không tìm thấy thông tin người dùng");
            }

            // Cập nhật thông tin người dùng - chỉ cập nhật trường nào có dữ liệu
            if (!string.IsNullOrEmpty(thongTin.HoTen)) userInfo.HoTen = thongTin.HoTen;
            if (!string.IsNullOrEmpty(thongTin.SoDienThoai)) userInfo.SoDienThoai = thongTin.SoDienThoai;
            if (!string.IsNullOrEmpty(thongTin.DiaChi)) userInfo.DiaChi = thongTin.DiaChi;
            if (!string.IsNullOrEmpty(thongTin.AnhDaiDien)) userInfo.AnhDaiDien = thongTin.AnhDaiDien;

            // Lưu thay đổi vào database
            _context.SaveChanges();

            // Cập nhật thông tin trong Session
            if (!string.IsNullOrEmpty(thongTin.HoTen)) HttpContext.Session.SetString("HoTen", thongTin.HoTen);
            if (!string.IsNullOrEmpty(thongTin.SoDienThoai)) HttpContext.Session.SetString("SoDienThoai", thongTin.SoDienThoai);
            if (!string.IsNullOrEmpty(thongTin.DiaChi)) HttpContext.Session.SetString("DiaChi", thongTin.DiaChi);
            if (!string.IsNullOrEmpty(thongTin.AnhDaiDien)) HttpContext.Session.SetString("AnhDaiDien", thongTin.AnhDaiDien ?? "");

            // Chuyển hướng về trang chủ sau khi chỉnh sửa thành công
            return RedirectToAction("Index", "TrangChu");
        }


        private bool ThongTinNguoiDungExists(int id)
        {
            return _context.ThongTinNguoiDung.Any(e => e.MaThongTinNguoiDung == id);
        }
    }
}
