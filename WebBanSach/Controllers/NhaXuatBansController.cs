using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Data;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class NhaXuatBansController : Controller
    {
        private readonly dbcontext _context;

        public NhaXuatBansController(dbcontext context)
        {
            _context = context;
        }

        // GET: NhaXuatBans
        public async Task<IActionResult> Index()
        {
              return _context.NhaXuatBans != null ? 
                          View(await _context.NhaXuatBans.ToListAsync()) :
                          Problem("Entity set 'dbcontext.NhaXuatBans'  is null.");
        }

        // GET: NhaXuatBans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.NhaXuatBans == null)
            {
                return NotFound();
            }

            var nhaXuatBan = await _context.NhaXuatBans
                .FirstOrDefaultAsync(m => m.ID_NXB == id);
            if (nhaXuatBan == null)
            {
                return NotFound();
            }

            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaXuatBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_NXB,TenXuatBan,TrangThai")] NhaXuatBan nhaXuatBan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(nhaXuatBan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("Error", "Something went wrong");
                    return View(nhaXuatBan);
                }
            }
            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NhaXuatBans == null)
            {
                return NotFound();
            }

            var nhaXuatBan = await _context.NhaXuatBans.FindAsync(id);
            if (nhaXuatBan == null)
            {
                return NotFound();
            }
            return View(nhaXuatBan);
        }

        // POST: NhaXuatBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID_NXB,TenXuatBan,TrangThai")] NhaXuatBan nhaXuatBan)
        {
            if (id != nhaXuatBan.ID_NXB)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {               
                try
                {
                    _context.Update(nhaXuatBan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!NhaXuatBanExists(nhaXuatBan.ID_NXB))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Something went wrong");
                        return View(nhaXuatBan);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.NhaXuatBans == null)
        //    {
        //        return NotFound();
        //    }

        //    var nhaXuatBan = await _context.NhaXuatBans
        //        .FirstOrDefaultAsync(m => m.ID_NXB == id);
        //    if (nhaXuatBan == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(nhaXuatBan);
        //}

        //// POST: NhaXuatBans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.NhaXuatBans == null)
        //    {
        //        return Problem("Entity set 'dbcontext.NhaXuatBans'  is null.");
        //    }
        //    var nhaXuatBan = await _context.NhaXuatBans.FindAsync(id);
        //    if (nhaXuatBan != null)
        //    {
        //        _context.NhaXuatBans.Remove(nhaXuatBan);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool NhaXuatBanExists(string id)
        {
          return (_context.NhaXuatBans?.Any(e => e.ID_NXB == id)).GetValueOrDefault();
        }
    }
}
