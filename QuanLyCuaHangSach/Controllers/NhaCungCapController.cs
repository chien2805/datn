﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var nhaCungCapList = await _context.NhaCungCap.ToListAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap == null)
                return NotFound();

            return View(nhaCungCap);
        }

        // POST: NhaCungCap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNhaCungCap,TenNhaCungCap,ThongTinLienHe")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.MaNhaCungCap)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaCungCap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaCungCapExists(nhaCungCap.MaNhaCungCap))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhaCungCap);
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
