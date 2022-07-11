using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Data;
using WebBanSach.Models;

namespace WebBanSach.Views
{
    [Authorize(Policy = "AdminOnly")]
    //[Authorize(AuthenticationSchemes = "MyCookie")]
    public class SachesController : Controller
    {
        
        private readonly dbcontext _context;
        Kho _kho;
       List <Kho> _lstKhos;
        private DateTime aDateTime;
        public SachesController(dbcontext context)
        {
            _context = context;
            _lstKhos = _context.Khos.ToList();
            _kho = new Kho();

        }

        // GET: Saches
        public async Task<IActionResult> Index()
        {
            var dbcontext = _context.Sachs.Include(s => s.NhaXuatBan);
            return View(await dbcontext.ToListAsync());
        }

        // GET: Saches/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Sachs == null)
            {
                return NotFound();
            }

            var sach = await _context.Sachs
                .Include(s => s.NhaXuatBan)
                .FirstOrDefaultAsync(m => m.ID_Sach == id);
            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }
        private List<Sach> GetSach()
        {
            List<Sach> saches = new List<Sach>();
            saches = _context.Sachs.ToList();
            return saches;
        }

        public List<SachCT> GetSachCT()
        {
            List<SachCT> sachCT = new List<SachCT>();
            sachCT = _context.SachCTs.ToList();
            return sachCT;
        }
        public List<TacGia> GetTG()
        {
            List<TacGia> tacGias = new List<TacGia>();
            tacGias = _context.TacGias.ToList();
            return tacGias;
        }
        public List<TheLoai> GetTL()
        {
            List<TheLoai> theLoais = new List<TheLoai>();
            theLoais = _context.TheLoais.ToList();
            return theLoais;
        }

        //GET: Saches/Create
        public class MultiDropDownListViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<SelectListItem> ItemList { get; set; }
        }
        public IActionResult Create()
        {
            MultiDropDownListViewModel model = new();
            ViewBag.IDTL =  model.ItemList = GetTL().Select(x => new SelectListItem { Text = x.TenTL, Value = x.ID_TheLoai.ToString() }).ToList();
        
            
            ViewBag.IDNXB = new SelectList(_context.NhaXuatBans, "ID_NXB", "TenXuatBan");
         
            ViewBag.IDTG = model.ItemList = GetTG().Select(x => new SelectListItem { Text = x.HoVaTen, Value = x.ID_TacGia.ToString() }).ToList();
            return View();
        }

        // POST: Saches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Sach,MaNXB,TenSach,HinhAnh,SoTrang,TaiBan,Gia,NgayNhap,SoLuong,TrangThai")] Sach sach, string[] SelectedIdsTL, string[] SelectedIdsTG, string gianhap)
        {
            if (sach != null)
            {
                try
                {
                    var nv = _context.NhanViens.Where(c => c.Email == User.Identity.Name).FirstOrDefault();
                    sach.ID_Sach = Guid.NewGuid().ToString();
                    sach.TrangThai = 1;
                    sach.NgayNhap = DateTime.Now;
                    _context.Add(sach);
                    //cường code
                    _kho.ID_Kho = Guid.NewGuid().ToString();
                    _kho.MaNhanVien = nv.ID_NhanVien;
                    _kho.MaSach = sach.ID_Sach;
                    _kho.SoLuong = sach.SoLuong;
                    _kho.NgayNhap = sach.NgayNhap;
                    _kho.GiaNhap = int.Parse(gianhap);
                    _kho.TrangThai = 1;
                    _context.Khos.Add(_kho);


                    foreach (var tgx in SelectedIdsTG)
                    {
                        foreach (var tlx in SelectedIdsTL)
                        {
                            SachCT sachCT = new SachCT();
                            sachCT.ID_SachCT = Guid.NewGuid().ToString();
                            sachCT.MaTacGia = tgx;
                            sachCT.MaTheLoai = tlx;
                            sachCT.TrangThai = 1;
                            sachCT.MaSach = sach.ID_Sach;
                            _context.Add(sachCT);
                        }
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    //ModelState.AddModelError("Error", "Something went wrong");
                    ViewBag.IDNXB = new SelectList(_context.NhaXuatBans, "ID_NXB", "TenXuatBan", sach.MaNXB);
                    return View(sach);
                }
            }

            ViewBag.IDNXB = new SelectList(_context.NhaXuatBans, "ID_NXB", "TenXuatBan", sach.MaNXB);
            return View(sach);
        }

        // GET: Saches/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Sachs == null)
            {
                return Content("Nope");
            }

            var sach = await _context.Sachs.FindAsync(id);
            if (sach == null)
            {
                return NotFound();
            }
            MultiDropDownListViewModel model = new();
            ViewBag.IDTL = model.ItemList = GetTL().Select(x => new SelectListItem { Text = x.TenTL, Value = x.ID_TheLoai.ToString(), Selected = _context.SachCTs.Where(s => s.MaSach == sach.ID_Sach).ToList().Exists(s => s.MaTheLoai == x.ID_TheLoai) }).ToList();
            ViewBag.IDNXB = new SelectList(_context.NhaXuatBans, "ID_NXB", "TenXuatBan",sach.MaNXB);
            ViewBag.IDTG = model.ItemList = GetTG().Select(x => new SelectListItem { Text = x.HoVaTen, Value = x.ID_TacGia.ToString(), Selected = _context.SachCTs.Where(s => s.MaSach == sach.ID_Sach).ToList().Exists(s => s.MaTacGia == x.ID_TacGia) }).ToList();
            return View(sach);
        }

        // POST: Saches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID_Sach,MaNXB,TenSach,HinhAnh,SoTrang,TaiBan,Gia,NgayNhap,SoLuong,TrangThai")] Sach sach, string[] SelectedIdsTL, string[] SelectedIdsTG)
        {
            if (id != sach.ID_Sach)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in SelectedIdsTG)
                    {
                        foreach (var item3 in SelectedIdsTL)
                        {
                            if (!_context.SachCTs.Where(c => c.MaSach == sach.ID_Sach).ToList().Exists(c => c.MaTacGia == item && c.MaTheLoai == item3))
                            {
                                SachCT sachCT = new SachCT();
                                sachCT.ID_SachCT = Guid.NewGuid().ToString();
                                sachCT.MaTacGia = item;
                                sachCT.MaTheLoai = item3;
                                sachCT.TrangThai = 1;
                                sachCT.MaSach = sach.ID_Sach;
                                _context.Add(sachCT);
                            }
                        }
                    }
                    foreach (var item2 in _context.SachCTs.Where(c => c.MaSach == sach.ID_Sach).ToList())
                    {
                        if (!SelectedIdsTG.Contains(item2.MaTacGia) || !SelectedIdsTL.Contains(item2.MaTheLoai))
                        {
                            _context.Remove(item2);
                        }
                    }
                    _context.Update(sach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!SachExists(sach.ID_Sach))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("Error","Something went wrong");
                        return View(sach);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IDNXB = new SelectList(_context.NhaXuatBans, "ID_NXB", "TenXuatBan", sach.MaNXB);
            return View(sach);
        }

        // GET: Saches/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.Sachs == null)
        //    {
        //        return NotFound();
        //    }

        //    var sach = await _context.Sachs
        //        .Include(s => s.NhaXuatBan)
        //        .FirstOrDefaultAsync(m => m.ID_Sach == id);
        //    if (sach == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sach);
        //}

        //// POST: Saches/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.Sachs == null)
        //    {
        //        return Problem("Entity set 'dbcontext.Sachs'  is null.");
        //    }
        //    var sach = await _context.Sachs.FindAsync(id);
        //    if (sach != null)
        //    {
        //        _context.Sachs.Remove(sach);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SachExists(string id)
        {
          return (_context.Sachs?.Any(e => e.ID_Sach == id)).GetValueOrDefault();
        }
    }
}
