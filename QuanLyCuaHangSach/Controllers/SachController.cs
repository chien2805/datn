using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Globalization;
using System.Text;

namespace QuanLyCuaHangSach.Controllers
{
    public class SachController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SachController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sach
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Số sách mỗi trang
            var sachList = await _context.Sach
                .Include(s => s.TheLoai)
                .Include(s => s.NhaCungCap)
                .Skip((page - 1) * pageSize) // Bỏ qua những sách đã được hiển thị trước đó
                .Take(pageSize) // Lấy số sách tương ứng với pageSize
                .ToListAsync();

            // Cập nhật ViewBag cho modal
            ViewBag.TheLoaiList = new SelectList(_context.TheLoai, "MaTheLoai", "TenTheLoai");
            ViewBag.NhaCungCapList = new SelectList(_context.NhaCungCap, "MaNhaCungCap", "TenNhaCungCap");

            // Tính tổng số trang
            var totalCount = await _context.Sach.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(sachList);
        }


        // GET: Sach/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var sach = await _context.Sach.Include(s => s.TheLoai).Include(s => s.NhaCungCap)
                                          .FirstOrDefaultAsync(m => m.MaSach == id);
            if (sach == null)
                return NotFound();
            var sachBanChay = _context.Sach
                .OrderByDescending(s => s.SoLuongBan)
                .Take(6)
                .ToList();
            var allBooks = _context.Sach.ToList();  // Lấy tất cả sách từ database

            // Lấy ngẫu nhiên 5 sách
            var randomBooks = allBooks.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            // Gửi vào ViewBag hoặc Model
            ViewBag.SachNoiBat = randomBooks;
            ViewBag.SachBanChay = sachBanChay;

            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> KiemTraTieuDe(string tieuDe)
        {
            bool tonTai = await _context.Sach.AnyAsync(s => s.TieuDe.ToLower() == tieuDe.ToLower());
            return Json(new { tonTai });
        }
        // GET: Sach/Create
        public IActionResult Create()
        {
            ViewBag.TheLoaiList = new SelectList(_context.TheLoai, "MaTheLoai", "TenTheLoai");
            ViewBag.NhaCungCapList = new SelectList(_context.NhaCungCap, "MaNhaCungCap", "TenNhaCungCap");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sach sach, IFormFile? HinhAnhFile)
        {
            // Kiểm tra nếu ModelState hợp lệ mới tiếp tục xử lý
            if (!ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Đảm bảo thư mục tồn tại trước khi lưu file
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(HinhAnhFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnhFile.CopyToAsync(stream);
                        }

                        // Gán đường dẫn file vào model (lưu đường dẫn tương đối)
                        sach.HinhAnh = "/images/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu file: " + ex.Message);
                        return View(sach);
                    }
                }

                _context.Add(sach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, load lại danh sách thể loại và nhà cung cấp
            ViewBag.TheLoaiList = new SelectList(_context.TheLoai, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            ViewBag.NhaCungCapList = new SelectList(_context.NhaCungCap, "MaNhaCungCap", "TenNhaCungCap", sach.MaNhaCungCap);
            return View(sach);
        }


        // GET: Sach/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var sach = await _context.Sach.FindAsync(id);
            if (sach == null)
                return NotFound();
            // Nếu đây là yêu cầu AJAX, trả về JSON
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(sach);
            }
            ViewBag.TheLoaiList = new SelectList(_context.TheLoai, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            ViewBag.NhaCungCapList = new SelectList(_context.NhaCungCap, "MaNhaCungCap", "TenNhaCungCap", sach.MaNhaCungCap);

            return View(sach);
        }

        [HttpGet]
        public IActionResult GetChiTiet(int id)
        {
            var sach = _context.Sach
                .Include(s => s.TheLoai)
                .Include(s => s.NhaCungCap)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null)
            {
                return NotFound();
            }

            var bookDetail = new
            {
                tieuDe = sach.TieuDe,
                hinhAnhUrl = sach.HinhAnh, // Đảm bảo đường dẫn đúng
                tacGia = sach.TacGia,
                tenTheLoai = sach.TheLoai.TenTheLoai,
                tenNhaCungCap = sach.NhaCungCap.TenNhaCungCap,
                nhaXuatBan = sach.NhaXuatBan,
                namXuatBan = sach.NamXuatBan,
                gia = sach.Gia,
                soLuongTon = sach.SoLuongTon,
                soLuongBan = sach.SoLuongBan,
                soLuongMuon = sach.SoLuongMuon,
                trangThai = sach.TrangThai,
                gioiThieu = sach.GioiThieu
            };

            return Json(bookDetail); // Trả về dữ liệu sách dưới dạng JSON
        }


        // POST: Sach/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Sach sach, IFormFile? HinhAnhFile)
        {
            if (!ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(HinhAnhFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    try
                    {
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await HinhAnhFile.CopyToAsync(stream);
                        sach.HinhAnh = "/images/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu file: " + ex.Message);
                        return View(sach);
                    }
                }

                _context.Update(sach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TheLoaiList = new SelectList(_context.TheLoai, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            ViewBag.NhaCungCapList = new SelectList(_context.NhaCungCap, "MaNhaCungCap", "TenNhaCungCap", sach.MaNhaCungCap);
            return View(sach);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var sach = await _context.Sach.FindAsync(id);
            if (sach == null) return NotFound();

            return Json(new
            {
                maSach = sach.MaSach,
                tieuDe = sach.TieuDe,
                tacGia = sach.TacGia,
                maTheLoai = sach.MaTheLoai,
                maNhaCungCap = sach.MaNhaCungCap,
                nhaXuatBan = sach.NhaXuatBan,
                namXuatBan = sach.NamXuatBan,
                gia = sach.Gia,
                soLuongTon = sach.SoLuongTon,
                soLuongBan = sach.SoLuongBan,
                soLuongMuon = sach.SoLuongMuon,
                trangThai = sach.TrangThai,
                gioiThieu = sach.GioiThieu
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sach = await _context.Sach.FindAsync(id);
            if (sach != null)
            {
                _context.Sach.Remove(sach);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Sach/GetSoLuongTon
        public IActionResult GetSoLuongTon(int maSach)
        {
            var sach = _context.Sach.FirstOrDefault(s => s.MaSach == maSach);
            if (sach != null)
            {
                return Json(new { soLuongTon = sach.SoLuongTon });
            }

            return Json(new { soLuongTon = 0 });
        }

        [HttpGet]
        public IActionResult KiemTraTonKho(int maSach)
        {
            var sach = _context.Sach.FirstOrDefault(s => s.MaSach == maSach);
            if (sach != null)
            {
                return Json(new { tonKho = sach.SoLuongTon });
            }
            return Json(new { tonKho = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> TimKiemSach(string term)
        {
            var sachList = await _context.Sach
                .Where(s => s.TieuDe.Contains(term))
                .Select(s => new { label = s.TieuDe, value = s.TieuDe })
                .ToListAsync();
            return Json(sachList);
        }

        [HttpGet]
        public async Task<IActionResult> LayGiaSach(string tenSach)
        {
            var sach = await _context.Sach.FirstOrDefaultAsync(s => s.TieuDe == tenSach);
            if (sach == null)
                return NotFound();

            return Json(new { gia = sach.Gia, maSach = sach.MaSach });
        }

        private bool SachExists(int id)
        {
            return _context.Sach.Any(e => e.MaSach == id);
        }
        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    // Bắt buộc: Thiết lập context cho EPPlus
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null) return RedirectToAction("Index");

                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string tieuDe = worksheet.Cells[row, 1].Text.Trim();
                            int soLuong = int.TryParse(worksheet.Cells[row, 2].Text.Trim(), out int sl) ? sl : 0;

                            if (!string.IsNullOrEmpty(tieuDe) && soLuong > 0)
                            {
                                string tieuDeChuanHoa = ChuanHoaChuoi(tieuDe);

                                // So sánh tên đã chuẩn hóa
                                var sach = _context.Sach
                                    .AsEnumerable()
                                    .FirstOrDefault(s => ChuanHoaChuoi(s.TieuDe) == tieuDeChuanHoa);

                                if (sach != null)
                                {
                                    sach.SoLuongTon += soLuong;
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        // Hàm chuẩn hóa chuỗi: xóa dấu và đưa về chữ thường
        public static string ChuanHoaChuoi(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            input = input.ToLower().Trim();

            string normalized = input.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
