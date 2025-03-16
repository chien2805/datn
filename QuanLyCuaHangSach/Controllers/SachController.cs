using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;

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
        public async Task<IActionResult> Index()
        {
            var sachList = await _context.Sach.Include(s => s.TheLoai).Include(s => s.NhaCungCap).ToListAsync();
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

            return View(sach);
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

            ViewBag.TheLoaiList = new SelectList(_context.TheLoai, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            ViewBag.NhaCungCapList = new SelectList(_context.NhaCungCap, "MaNhaCungCap", "TenNhaCungCap", sach.MaNhaCungCap);

            return View(sach);
        }

        // POST: Sach/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSach,TieuDe,TacGia,MaTheLoai,MaNhaCungCap,NhaXuatBan,NamXuatBan,SoLuongBan,SoLuongMuon,SoLuongTon,SoLuongHong,SoLuongMat,Gia,TrangThai")] Sach sach)
        {
            if (id != sach.MaSach)
                return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(sach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SachExists(sach.MaSach))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sach);
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
    }
}
