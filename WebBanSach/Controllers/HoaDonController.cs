using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Data;
using WebBanSach.Models;
using WebBanSach.ModelView;

namespace WebBanSach.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class HoaDonController : Controller
    {
        private List<KhachHang> _listKH;
        private List<HoaDonCT> _listHDCT;
        private List<Sach> _listS;
        private List<HoaDon> _listHD;
        private List<ViewHoaDon> _listviewhd;
        private List<NhaXuatBan> _listNXB;
        private List<TacGia> _listTG;
        private List<TheLoai> _listTL;
        private List<SachCT> _listSCT;
        private dbcontext _db;
        public HoaDonController(dbcontext dbcontext)
        {
            _db = dbcontext;
            _listKH = new List<KhachHang>();
            _listHDCT = new List<HoaDonCT>();
            _listS = new List<Sach>();
            _listHD = new List<HoaDon>();
            _listviewhd = new List<ViewHoaDon>();
            _listNXB = new List<NhaXuatBan>();
            _listTG = new List<TacGia>();
            _listTL = new List<TheLoai>();
            _listSCT = new List<SachCT>();

        }
        public ActionResult Sua(string dd, string name)
        {

            if (dd == null || name == null)
            {
                return NotFound();
            }
            var i = _db.HoaDons.Where(c => c.ID_HoaDon == dd).FirstOrDefault();

            if (name == "1")
            {
                i.TrangThai = 1;


            }
            else if (name == "2")
            {
                i.TrangThai = 0;

            }
            else
            {
                i.TrangThai = 2;
            }
            _db.Update(i);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));








            //return View(listhd());



        }
        // GET: HoaDonController
        public ActionResult Index(string SearchString, DateTime? star, DateTime? end)
        {
            _listHD = _db.HoaDons.ToList();
            _listHDCT = _db.HoaDonCTs.ToList();
            _listKH = _db.KhachHangs.ToList();
            _listS = _db.Sachs.ToList();

            var listviewhd = (from a in _listHD
                              join b in _listHDCT on a.ID_HoaDon equals b.MaHoaDon
                              join d in _listKH on a.MaKH equals d.ID_KhachHang
                              join c in _listS on b.MaSach equals c.ID_Sach

                              select new ViewHoaDon()
                              {
                                  hoaDon = a,
                                  khachHang = d,
                                  hoaDonCT = b,
                                  sach = c
                              }).ToList();
            var list = listviewhd.OrderByDescending(a => a.hoaDon.NgayMua).ToList();


            if (!String.IsNullOrEmpty(SearchString))
            {
                listviewhd = listviewhd.Where(s => s.khachHang.HoVaTen.Contains(SearchString)).ToList();
            }
            if (star != null && end != null)
            {
                listviewhd = listviewhd.Where(x => x.hoaDon.NgayMua <= star && x.hoaDon.NgayMua >= end).ToList();
                return View(listviewhd);
            }
            return View(listviewhd);
        }
        public ActionResult HoaDonCT(string SearchString, DateTime? star, DateTime? end)
        {
            _listHD = _db.HoaDons.ToList();
            _listHDCT = _db.HoaDonCTs.ToList();
            _listKH = _db.KhachHangs.ToList();
            _listS = _db.Sachs.ToList();
            _listTL = _db.TheLoais.ToList();
            _listNXB = _db.NhaXuatBans.ToList();
            _listTG = _db.TacGias.ToList();
            _listSCT = _db.SachCTs.ToList();

            var listviewhd = (from a in _listHD
                              join b in _listHDCT on a.ID_HoaDon equals b.MaHoaDon
                              join d in _listKH on a.MaKH equals d.ID_KhachHang
                              join c in _listS on b.MaSach equals c.ID_Sach
                              join v in _listSCT on c.ID_Sach equals v.MaSach
                              join n in _listTG on v.MaTacGia equals n.ID_TacGia
                              join m in _listTL on v.MaTheLoai equals m.ID_TheLoai
                              join k in _listNXB on c.MaNXB equals k.ID_NXB
                              select new ViewHoaDon()
                              {
                                  hoaDon = a,
                                  khachHang = d,
                                  hoaDonCT = b,
                                  sach = c,
                                  sachCT = v,
                                  tacGia = n,
                                  TheLoai = m,
                                  nhaXuatBan = k
                              }).ToList();
            var list = listviewhd.OrderByDescending(a => a.hoaDon.NgayMua).ToList();


            if (!String.IsNullOrEmpty(SearchString))
            {
                listviewhd = listviewhd.Where(s => s.khachHang.HoVaTen.Contains(SearchString)).ToList();
            }
            if (star != null && end != null)
            {
                listviewhd = listviewhd.Where(x => x.hoaDon.NgayMua <= star && x.hoaDon.NgayMua >= end).ToList();
                return View(listviewhd);
            }
            return View(listviewhd);


        }
        public List<ViewHoaDon> listhd()
        {
            _listHD = _db.HoaDons.ToList();
            _listHDCT = _db.HoaDonCTs.ToList();
            _listKH = _db.KhachHangs.ToList();
            _listS = _db.Sachs.ToList();
            _listTL = _db.TheLoais.ToList();
            _listNXB = _db.NhaXuatBans.ToList();
            _listTG = _db.TacGias.ToList();
            _listSCT = _db.SachCTs.ToList();

            var listviewhd = (from a in _listHD
                              join b in _listHDCT on a.ID_HoaDon equals b.MaHoaDon
                              join d in _listKH on a.MaKH equals d.ID_KhachHang
                              join c in _listS on b.MaSach equals c.ID_Sach
                              join v in _listSCT on c.ID_Sach equals v.MaSach
                              join n in _listTG on v.MaTacGia equals n.ID_TacGia
                              join m in _listTL on v.MaTheLoai equals m.ID_TheLoai
                              join k in _listNXB on c.MaNXB equals k.ID_NXB
                              select new ViewHoaDon()
                              {
                                  hoaDon = a,
                                  khachHang = d,
                                  hoaDonCT = b,
                                  sach = c,
                                  sachCT = v,
                                  tacGia = n,
                                  TheLoai = m,
                                  nhaXuatBan = k
                              }).ToList();
            return listviewhd;
        }


        // GET: HoaDonController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HoaDonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HoaDonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HoaDonController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HoaDonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HoaDonController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HoaDonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
