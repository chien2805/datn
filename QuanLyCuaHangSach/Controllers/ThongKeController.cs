using QuanLyCuaHangSach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using QuanLyCuaHangSach.Context;
using Newtonsoft.Json;

namespace QuanLyCuaHangSach.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThongKeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? month, int? year)
        {
            // Nếu không có month/year thì dùng giá trị hiện tại
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;

            // Truyền tháng, năm sang ViewBag
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;

            // Lấy dữ liệu doanh thu theo ngày từ 3 model
            var banOnlineData = _context.HoaDonBanOnline
                .Where(h => h.NgayTao.Month == selectedMonth && h.NgayTao.Year == selectedYear)
                .GroupBy(h => h.NgayTao.Date)
                .Select(g => new
                {
                    ngay = g.Key,
                    tongTien = g.Sum(x => x.TongTien)
                })
                .ToList();

            var banQuayData = _context.HoaDonBan
                .Where(h => h.NgayLap.Month == selectedMonth && h.NgayLap.Year == selectedYear)
                .GroupBy(h => h.NgayLap.Date)
                .Select(g => new
                {
                    ngay = g.Key,
                    tongTien = g.Sum(x => x.TongTien)
                })
                .ToList();

            var muonSachData = _context.PhieuDatTruoc
                .Where(p => p.TrangThai == "Đã trả" && p.NgayTra.Month == selectedMonth && p.NgayTra.Year == selectedYear)
                .GroupBy(p => p.NgayTra.Date)
                .Select(g => new
                {
                    ngay = g.Key,
                    tongTien = g.Sum(x => x.ThanhTien ?? 0)
                })
                .ToList();

            // Tạo danh sách tất cả các ngày trong tháng đã chọn
            var startDate = new DateTime(selectedYear, selectedMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var allDates = GenerateAllDates(startDate, endDate);

            // Kết hợp dữ liệu với danh sách ngày đầy đủ
            var labels = new List<string>();
            var tongTienBanOnlineList = new List<decimal>();
            var tongTienBanQuayList = new List<decimal>();
            var tongTienMuonSachList = new List<decimal>();

            foreach (var date in allDates)
            {
                var formattedDate = date.ToString("yyyy-MM-dd");

                var itemOnline = banOnlineData.FirstOrDefault(b => b.ngay.ToString("yyyy-MM-dd") == formattedDate);
                var itemQuay = banQuayData.FirstOrDefault(b => b.ngay.ToString("yyyy-MM-dd") == formattedDate);
                var itemMuon = muonSachData.FirstOrDefault(m => m.ngay.ToString("yyyy-MM-dd") == formattedDate);

                labels.Add(formattedDate);
                tongTienBanOnlineList.Add(itemOnline?.tongTien ?? 0);
                tongTienBanQuayList.Add(itemQuay?.tongTien ?? 0);
                tongTienMuonSachList.Add(itemMuon?.tongTien ?? 0);
            }

            // Tính tổng doanh thu của tháng
            decimal totalRevenueOnline = tongTienBanOnlineList.Sum();
            decimal totalRevenueQuay = tongTienBanQuayList.Sum();
            decimal totalRevenueMuon = tongTienMuonSachList.Sum();
            decimal totalRevenueMonth = totalRevenueOnline + totalRevenueQuay + totalRevenueMuon;

            // Tính tổng số sách bán (cộng online và bán tại quầy)
            // Giả sử HoaDonBanOnline có navigation property ChiTietHoaDonOnline,
            // và HoaDonBan có navigation property ChiTietHoaDon.
            int totalBooksSoldOnline = _context.ChiTietHoaDonOnline
                .Where(ct => ct.HoaDon.NgayTao.Month == selectedMonth && ct.HoaDon.NgayTao.Year == selectedYear)
                .Sum(ct => ct.SoLuong);
            int totalBooksSoldQuay = _context.ChiTietHoaDon
                .Where(ct => ct.HoaDon.NgayLap.Month == selectedMonth && ct.HoaDon.NgayLap.Year == selectedYear)
                .Sum(ct => ct.SoLuong);
            int totalBooksSold = totalBooksSoldOnline + totalBooksSoldQuay;

            // Tính tổng số sách mượn
            int totalBooksBorrowed = _context.ChiTietPhieuDatTruoc
                .Where(ct => ct.PhieuDatTruoc.NgayTra.Month == selectedMonth && ct.PhieuDatTruoc.NgayTra.Year == selectedYear)
                .Sum(ct => ct.SoLuongMuon);

            // Truyền dữ liệu vào ViewBag
            ViewBag.Labels = JsonConvert.SerializeObject(labels);
            ViewBag.BanOnlineData = JsonConvert.SerializeObject(tongTienBanOnlineList);
            ViewBag.BanQuayData = JsonConvert.SerializeObject(tongTienBanQuayList);
            ViewBag.MuonSachData = JsonConvert.SerializeObject(tongTienMuonSachList);

            // Truyền các tổng số liệu cho tháng
            ViewBag.TotalBooksSold = totalBooksSold;
            ViewBag.TotalBooksBorrowed = totalBooksBorrowed;
            ViewBag.TotalRevenue = totalRevenueMonth;

            return View();
        }

        public List<DateTime> GenerateAllDates(DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                dates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return dates;
        }
    }
}
