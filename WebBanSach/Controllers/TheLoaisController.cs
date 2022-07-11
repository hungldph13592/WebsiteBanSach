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
    public class TheLoaisController : Controller
    {
        private readonly dbcontext _context;

        public TheLoaisController(dbcontext context)
        {
            _context = context;
        }

        // GET: TheLoais
        public async Task<IActionResult> Index()
        {
            return View(await _context.TheLoais.ToListAsync());
        }

        // GET: TheLoais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theLoai = await _context.TheLoais
                .FirstOrDefaultAsync(m => m.ID_TheLoai == id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return View(theLoai);
        }

        // GET: TheLoais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TheLoais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_TheLoai,TenTL")] TheLoai theLoai)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(theLoai);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Error", "Something went wrong");
                return View(theLoai);
            }
            return View(theLoai);
        }

        // GET: TheLoais/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theLoai = await _context.TheLoais.FindAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }
            return View(theLoai);
        }

        // POST: TheLoais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID_TheLoai,TenTL")] TheLoai theLoai)
        {
            if (id != theLoai.ID_TheLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theLoai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!TheLoaiExists(theLoai.ID_TheLoai))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Something went wrong");
                        return View(theLoai);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }

        // GET: TheLoais/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theLoai = await _context.TheLoais
                .FirstOrDefaultAsync(m => m.ID_TheLoai == id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return View(theLoai);
        }

        // POST: TheLoais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var theLoai = await _context.TheLoais.FindAsync(id);
            _context.TheLoais.Remove(theLoai);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheLoaiExists(string id)
        {
            return _context.TheLoais.Any(e => e.ID_TheLoai == id);
        }
    }
}
