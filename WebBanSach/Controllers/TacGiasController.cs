#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Data;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class TacGiasController : Controller
    {
        private readonly dbcontext _context;

        public TacGiasController(dbcontext context)
        {
            _context = context;
        }

        // GET: TacGias
        public async Task<IActionResult> Index()
        {
            return View(await _context.TacGias.ToListAsync());
        }

        // GET: TacGias/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tacGia = await _context.TacGias
                .FirstOrDefaultAsync(m => m.ID_TacGia == id);
            if (tacGia == null)
            {
                return NotFound();
            }

            return View(tacGia);
        }

        // GET: TacGias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TacGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_TacGia,HoVaTen,NgaySinh,QueQuan,TrangThai")] TacGia tacGia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tacGia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tacGia);
        }

        // GET: TacGias/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tacGia = await _context.TacGias.FindAsync(id);
            if (tacGia == null)
            {
                return NotFound();
            }
            return View(tacGia);
        }

        // POST: TacGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID_TacGia,HoVaTen,NgaySinh,QueQuan,TrangThai")] TacGia tacGia)
        {
            if (id != tacGia.ID_TacGia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (tacGia.TrangThai == 0)
                    {
                        foreach (var item in _context.SachCTs.Where(c => c.MaTacGia == tacGia.ID_TacGia).ToList())
                        {
                            _context.Remove(item);
                        }
                    }
                    _context.Update(tacGia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TacGiaExists(tacGia.ID_TacGia))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tacGia);
        }

        //// GET: TacGias/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tacGia = await _context.TacGias
        //        .FirstOrDefaultAsync(m => m.ID_TacGia == id);
        //    if (tacGia == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tacGia);
        //}

        //// POST: TacGias/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var tacGia = await _context.TacGias.FindAsync(id);
        //    _context.TacGias.Remove(tacGia);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TacGiaExists(string id)
        {
            return _context.TacGias.Any(e => e.ID_TacGia == id);
        }
    }
}
