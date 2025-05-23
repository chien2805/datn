using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;

namespace QuanLyCuaHangSach.Controllers
{
    public class NhaCungCapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhaCungCapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NhaCungCap
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Số lượng items hiển thị trên mỗi trang
            var totalItems = await _context.NhaCungCap.CountAsync(); // Đếm tổng số items
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Tính tổng số trang

            // Lấy dữ liệu cho trang hiện tại
            var nhaCungCapList = await _context.NhaCungCap
                .Skip((page - 1) * pageSize) // Bỏ qua số lượng items của các trang trước
                .Take(pageSize) // Lấy số lượng items theo pageSize
                .ToListAsync();

            // Cập nhật ViewBag để truyền dữ liệu phân trang vào view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(nhaCungCapList);
        }


        // GET: NhaCungCap/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaCungCap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenNhaCungCap,ThongTinLienHe")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaCungCap);
                await _context.SaveChangesAsync();

                // Nếu là AJAX request (từ modal), trả về JSON success
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }

                // Nếu không phải AJAX, redirect về Index
                return RedirectToAction(nameof(Index));
            }

            // Nếu model không hợp lệ
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Gom tất cả lỗi validate lại để trả về client
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();
                return Json(new { success = false, errors });
            }

            // Nếu submit bình thường và có lỗi, trả về lại view Create để hiển thị lỗi
            return View(nhaCungCap);
        }


        // GET: NhaCungCap/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);

            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return Json(nhaCungCap);  // Trả về dữ liệu dưới dạng JSON cho AJAX
        }

        // POST: NhaCungCap/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNhaCungCap,TenNhaCungCap,ThongTinLienHe")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.MaNhaCungCap)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaCungCap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaCungCapExists(nhaCungCap.MaNhaCungCap))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCap/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCap
                .FirstOrDefaultAsync(m => m.MaNhaCungCap == id);

            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return Json(nhaCungCap); // Trả về dữ liệu dưới dạng JSON cho AJAX
        }



        // GET: NhaCungCap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var nhaCungCap = await _context.NhaCungCap.FirstOrDefaultAsync(m => m.MaNhaCungCap == id);
            if (nhaCungCap == null)
                return NotFound();

            return View(nhaCungCap);
        }

        // POST: NhaCungCap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap != null)
            {
                _context.NhaCungCap.Remove(nhaCungCap);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NhaCungCapExists(int id)
        {
            return _context.NhaCungCap.Any(e => e.MaNhaCungCap == id);
        }
    }
}
