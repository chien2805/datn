using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangSach.Controllers
{
    public class PhieuDatTruocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhieuDatTruocController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            // Thử lấy MaTaiKhoan từ Claims
            var maTaiKhoanClaim = User.FindFirst("MaTaiKhoan")?.Value;
            var vaiTro = User.FindFirst(ClaimTypes.Role)?.Value;

            // Nếu không có trong Claims, thử lấy từ Session
            if (string.IsNullOrEmpty(maTaiKhoanClaim))
            {
                maTaiKhoanClaim = HttpContext.Session.GetString("MaTaiKhoan");
            }

            // Nếu vẫn không có, chuyển hướng
            if (string.IsNullOrEmpty(maTaiKhoanClaim) || vaiTro == "Khach Hang")
            {
                return RedirectToAction("Index", "TrangChu"); // hoặc trang lỗi
            }

            int maTaiKhoan = int.Parse(maTaiKhoanClaim);

            // Lấy danh sách phiếu và sắp xếp từ mới đến cũ
            var danhSachPhieu = _context.PhieuDatTruoc
                .Include(p => p.TaiKhoanNguoiDung)
                    .ThenInclude(n => n.ThongTinNguoiDung)
                .Include(p => p.ChiTietPhieuDatTruoc)
                    .ThenInclude(ct => ct.Sach)
                .OrderByDescending(p => p.NgayDat) // Sắp xếp từ mới đến cũ
                .ToList();

            // Lấy 10 bản ghi trên mỗi trang
            int pageSize = 10;
            var pagedData = danhSachPhieu.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Lấy tổng số trang
            var totalPages = (int)Math.Ceiling(danhSachPhieu.Count / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedData);
        }


        //
        [HttpPost]
        public IActionResult DuyetPhieu(int id)
        {
            var phieu = _context.PhieuDatTruoc.Find(id);
            /*if (phieu == null)
            {
                return NotFound();
            }*/

            // Cập nhật trạng thái phiếu đặt trước
            phieu.TrangThai = "Đã xử lý";
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Phiếu đặt trước đã được duyệt.";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult XacThucNguoiDung(int maNguoiDung)
        {
            // Kiểm tra nếu maNguoiDung không hợp lệ
            if (maNguoiDung <= 0)
            {
                return BadRequest("Người dùng không hợp lệ");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var thongTin = _context.ThongTinNguoiDung.FirstOrDefault(t => t.MaTaiKhoan == maNguoiDung);
            if (thongTin == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng");
            }

            // Trả về thông tin người dùng
            return Json(new
            {
                success = true,
                hoTen = thongTin.HoTen ?? "Không có dữ liệu",
                soDienThoai = thongTin.SoDienThoai ?? "Không có dữ liệu"
            });
        }



        [HttpPost]
        public IActionResult XacThucSach(List<int> danhSachMaSach)
        {
            if (danhSachMaSach == null || !danhSachMaSach.Any())
            {
                return BadRequest("Danh sách sách không hợp lệ");
            }

            // Debugging: In ra các mã sách để kiểm tra
            Console.WriteLine($"Danh sách mã sách: {string.Join(", ", danhSachMaSach)}");

            var danhSachSach = _context.Sach.Where(s => danhSachMaSach.Contains(s.MaSach)).ToList();
            if (!danhSachSach.Any())
            {
                return NotFound("Không tìm thấy sách nào");
            }

            return Json(new
            {
                success = true,
                danhSachSach = danhSachSach.Select(s => new { s.MaSach, s.TieuDe })
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TaoPhieu(int maSach, int maTaiKhoan)
        {
            /*// Kiểm tra xem người dùng có hợp lệ không
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || maTaiKhoan.ToString() != userId)
            {
                return Unauthorized();
            }*/

            // Kiểm tra sách có tồn tại không
            var sach = _context.Sach.Find(maSach);
            if (sach == null)
            {
                return NotFound("Sách không tồn tại.");
            }

            // Kiểm tra tồn kho
            if (sach.SoLuongTon <= 0)
            {
                TempData["Error"] = "Sách đã hết hàng.";
                return RedirectToAction("Index", "Sach");
            }

            // Tạo phiếu đặt trước
            var phieu = new PhieuDatTruoc
            {
                MaTaiKhoan = maTaiKhoan,
                NgayDat = DateTime.Now,
                NgayTra = DateTime.Now.AddDays(7), // Mặc định trả sau 7 ngày
                NgayTraThucTe = null,              // Chưa trả, nên để null
                ThanhTien = 7000,
                TrangThai = "Đang xử lý"
            };

            _context.PhieuDatTruoc.Add(phieu);
            _context.SaveChanges();

            // Tạo chi tiết phiếu đặt trước
            var chiTiet = new ChiTietPhieuDatTruoc
            {
                MaPhieuDatTruoc = phieu.MaPhieuDatTruoc,
                MaSach = maSach,
                SoLuongMuon = 1, // Mặc định mượn 1 cuốn
                
            };

            _context.ChiTietPhieuDatTruoc.Add(chiTiet);

            // Giảm số lượng tồn kho của sách
            sach.SoLuongTon -= 1;
            _context.SaveChanges();

            TempData["MaPhieuThanhCong"] = phieu.MaPhieuDatTruoc;
            return RedirectToAction("Index", "TrangChu");
        }


        [HttpPost]
        public IActionResult XacNhan(int maPhieu)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var phieu = _context.PhieuDatTruoc
                        .Include(p => p.ChiTietPhieuDatTruoc)
                        .ThenInclude(c => c.Sach)
                        .FirstOrDefault(p => p.MaPhieuDatTruoc == maPhieu);

                    if (phieu == null)
                    {
                        return NotFound("Không tìm thấy phiếu đặt trước.");
                    }

                    if (phieu.TrangThai != "ChoXacNhan")
                    {
                        return BadRequest("Phiếu không hợp lệ để xác nhận.");
                    }

                    foreach (var chiTiet in phieu.ChiTietPhieuDatTruoc)
                    {
                        if (chiTiet.Sach == null || chiTiet.Sach.SoLuongTon < chiTiet.SoLuongMuon)
                        {
                            return BadRequest($"Sách {chiTiet.Sach?.TieuDe} không đủ số lượng tồn.");
                        }

                        chiTiet.Sach.SoLuongTon -= chiTiet.SoLuongMuon;
                    }

                    phieu.TrangThai = "DaXacNhan";
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return StatusCode(500, "Có lỗi xảy ra khi xác nhận phiếu.");
                }
            }

            return Ok();
        }


        [HttpPost]
        public IActionResult DaTra(int id)
        {
            var phieu = _context.PhieuDatTruoc
                .Include(p => p.ChiTietPhieuDatTruoc)
                .ThenInclude(c => c.Sach)
                .FirstOrDefault(p => p.MaPhieuDatTruoc == id);

            if (phieu == null)
            {
                return NotFound("Không tìm thấy phiếu đặt trước.");
            }

            if (phieu.TrangThai != "Đã xử lý")
            {
                return BadRequest("Phiếu không hợp lệ để cập nhật trạng thái đã trả.");
            }

            // Cập nhật số lượng tồn của sách
            foreach (var chiTiet in phieu.ChiTietPhieuDatTruoc)
            {
                if (chiTiet.Sach != null)
                {
                    chiTiet.Sach.SoLuongTon += chiTiet.SoLuongMuon; // Tăng lại số lượng tồn
                    chiTiet.Sach.SoLuongMuon += chiTiet.SoLuongMuon; // Tăng số lần mượn
                }
            }

            // Cập nhật trạng thái

            // Ghi lại ngày trả thực tế và cập nhật trạng thái
            phieu.NgayTraThucTe = DateTime.Now;
            phieu.TrangThai = "Đã trả";
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult Huy(int id)
        {
            var phieu = _context.PhieuDatTruoc
                .Include(p => p.ChiTietPhieuDatTruoc)
                .ThenInclude(c => c.Sach)
                .FirstOrDefault(p => p.MaPhieuDatTruoc == id);

            /*if (phieu == null)
            {
                return NotFound("Không tìm thấy phiếu đặt trước.");
            }*/

            if (phieu.TrangThai != "Đang xử lý")
            {
                return BadRequest("Chỉ có thể hủy phiếu khi đang chờ xác nhận.");
            }

            // Hoàn lại số lượng sách đã đặt trước vào tồn kho
            foreach (var chiTiet in phieu.ChiTietPhieuDatTruoc)
            {
                if (chiTiet.Sach != null)
                {
                    chiTiet.Sach.SoLuongTon += chiTiet.SoLuongMuon; // Cộng lại vào tồn kho
                }
            }

            // Cập nhật trạng thái thành "Huy"
            phieu.TrangThai = "Đã hủy";
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult TimKiemSach(string term)
        {
            var ketQua = _context.Sach
                .Where(s => s.TieuDe.Contains(term))
                .Select(s => new {
                    label = s.TieuDe,
                    maSach = s.MaSach
                }).Take(10).ToList();

            return Json(ketQua);
        }

        [HttpPost]
        public IActionResult TaoTaiQuay(string TenKhachHang, string SoDienThoai, string JsonChiTiet, decimal ThanhTien)
        {
            var chiTietList = JsonConvert.DeserializeObject<List<ChiTietPhieuDatTruoc>>(JsonChiTiet);

            // Trừ số lượng tồn
            foreach (var ct in chiTietList)
            {
                var sach = _context.Sach.FirstOrDefault(s => s.MaSach == ct.MaSach);
                if (sach != null)
                {
                    if (sach.SoLuongTon >= ct.SoLuongMuon)
                    {
                        sach.SoLuongTon -= ct.SoLuongMuon;
                    }
                    else
                    {
                        // Nếu không đủ tồn kho, có thể xử lý lỗi hoặc bỏ qua
                        TempData["Error"] = $"Sách '{sach.TieuDe}' không đủ số lượng tồn.";
                        return RedirectToAction("Index");
                    }
                }
            }

            var phieu = new PhieuDatTruoc
            {
                TenKhachHang = TenKhachHang,
                SoDienThoai = SoDienThoai,
                NgayDat = DateTime.Now,
                NgayTra = DateTime.Now.AddDays(7),
                TrangThai = "Đã xử lý",
                ThanhTien = ThanhTien,
                ChiTietPhieuDatTruoc = chiTietList
            };

            _context.PhieuDatTruoc.Add(phieu);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
