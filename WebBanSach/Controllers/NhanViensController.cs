using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Data;
using WebBanSach.Models;


namespace WebBanSach.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class NhanViensController : Controller
    {
        private readonly dbcontext _context;

        public NhanViensController(dbcontext context)
        {
            _context = context;
        }

        // GET: NhanViens

        public async Task<IActionResult> Index(string SearchString, DateTime? star, DateTime? end)
        {
            var l = from a in _context.NhanViens select a;

            if (!String.IsNullOrEmpty(SearchString))
            {
                l = l.Where(s => s.HoVaTen.Contains(SearchString));
            }

            if (star != null && end != null)
            {
                var c = from x in _context.NhanViens where (x.NgaySinh <= star) && (x.NgaySinh >= end) select x;
                return View(await c.ToListAsync());
            }

            return View(await l.ToListAsync());

        }



        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.ID_NhanVien == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public IActionResult Create()
        {
            return View();
        }
        public static void CreateIfMissing(string path)
        {
            bool a = Directory.Exists(path);
            if (!a)
            {
                Directory.CreateDirectory(path);
            }
        }
        public static string Totitlecase(string tr)
        {
            string result = tr;
            if (!string.IsNullOrEmpty(tr))
            {
                var words = tr.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static async Task<string> UpLoadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                var supportedtypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileext = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedtypes.Contains(fileext.ToLower()))
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathfile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.

        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoVaTen,Email,SDT,DiaChi,HinhAnh,NgaySinh,MatKhau,Quyen,TrangThai")] NhanVien nhanVien, IFormFile fanh)
        {
            if (fanh != null)
            {
                nhanVien.HoVaTen = Totitlecase(nhanVien.HoVaTen);
                if (fanh != null)
                {
                    string extensoin = Path.GetExtension(fanh.FileName);
                    string image = nhanVien.HoVaTen + extensoin;
                    nhanVien.HinhAnh = await UpLoadFile(fanh, @"Nhanviens", image.ToLower());
                }

                if (string.IsNullOrEmpty(nhanVien.HinhAnh)) nhanVien.HinhAnh = "default.jpg";
                var a = _context.NhanViens.ToList();
                nhanVien.ID_NhanVien = "NV" + a.Count;
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID_NhanVien,HoVaTen,Email,SDT,DiaChi,HinhAnh,NgaySinh,MatKhau,Quyen,TrangThai")] NhanVien nhanVien)
        {
            if (id != nhanVien.ID_NhanVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.ID_NhanVien))
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
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.ID_NhanVien == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.NhanViens == null)
            {
                return Problem("Entity set 'dbcontext.NhanViens'  is null.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(string id)
        {
            return (_context.NhanViens?.Any(e => e.ID_NhanVien == id)).GetValueOrDefault();
        }
    }
}
