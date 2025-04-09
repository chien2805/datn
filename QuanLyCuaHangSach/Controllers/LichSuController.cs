using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models; // Namespace chứa model và ViewModel

namespace QuanLyCuaHangSach.Controllers
{
    public class LichSuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LichSuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            // Lịch sử đặt trước
            var lichSuDatTruoc = _context.PhieuDatTruoc
                .Where(p => p.MaTaiKhoan == maTaiKhoan)
                .Include(p => p.ChiTietPhieuDatTruoc)
                    .ThenInclude(ct => ct.Sach)
                .OrderByDescending(p => p.NgayDat)
                .ToList();

            // Lịch sử hóa đơn online
            var hoaDonOnline = _context.HoaDonBanOnline
                .Where(h => h.MaTaiKhoan == maTaiKhoan)
                .Include(h => h.ChiTietHoaDon)
                    .ThenInclude(ct => ct.Sach)
                .OrderByDescending(h => h.NgayTao)
                .ToList();

            var viewModel = new LichSuViewModel
            {
                LichSuMuon = lichSuDatTruoc,
                LichSuMua = hoaDonOnline
            };

            return View(viewModel);
        }
    }
}
