using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;

namespace QuanLyCuaHangSach.Controllers
{
    public class TheLoaiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TheLoaiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult KiemTraTenTheLoai(string tenTheLoai)
        {
            bool daTonTai = _context.TheLoai.Any(t => t.TenTheLoai.ToLower() == tenTheLoai.ToLower());
            return Json(!daTonTai); // Trả về true nếu không tồn tại (hợp lệ)
        }
        // GET: TheLoai
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Số lượng items hiển thị trên mỗi trang
            var totalItems = await _context.TheLoai.CountAsync(); // Đếm tổng số items
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Tính tổng số trang

            // Lấy dữ liệu cho trang hiện tại
            var theLoaiList = await _context.TheLoai
                .Skip((page - 1) * pageSize) // Bỏ qua số lượng items của các trang trước
                .Take(pageSize) // Lấy số lượng items theo pageSize
                .ToListAsync();

            // Cập nhật ViewBag để truyền dữ liệu phân trang vào view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(theLoaiList);
        }


        // GET: TheLoai/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenTheLoai,MoTa")] TheLoai theLoai)
        {
            Console.WriteLine($"Tên thể loại: {theLoai.TenTheLoai}");
            Console.WriteLine($"Mô tả: {theLoai.MoTa}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(theLoai);
            }

            _context.Add(theLoai);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TheLoai/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var theLoai = await _context.TheLoai.FindAsync(id);
            if (theLoai == null)
                return NotFound();

            return View(theLoai);
        }

        // POST: TheLoai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTheLoai,TenTheLoai,MoTa")] TheLoai theLoai)
        {
            if (id != theLoai.MaTheLoai)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theLoai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheLoaiExists(theLoai.MaTheLoai))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }

        // GET: TheLoai/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var theLoai = await _context.TheLoai.FirstOrDefaultAsync(m => m.MaTheLoai == id);
            if (theLoai == null)
                return NotFound();

            return View(theLoai);
        }

        // POST: TheLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theLoai = await _context.TheLoai.FindAsync(id);
            if (theLoai != null)
            {
                _context.TheLoai.Remove(theLoai);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool TheLoaiExists(int id)
        {
            return _context.TheLoai.Any(e => e.MaTheLoai == id);
        }
    }
}
