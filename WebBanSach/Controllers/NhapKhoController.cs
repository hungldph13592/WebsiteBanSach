using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Models;
using WebBanSach.ModelGhep;
using WebBanSach.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebBanSach.Controllers
{
    [Authorize(Policy = "StaffOnly")]
    public class NhapKhoController : Controller
    {
        public readonly dbcontext _dbcontext;
        List<Sach> _lstsach;
        List<SachCT> _lstsachCT;
        List<NhaXuatBan> _lstnhaXuatBans;
        List<TacGia> _lsttacGias;
        List<TheLoai> _lsttheLoais;
        List<SanPhamview> _lstsanPhamviews;
        List<Kho> _lstKho;
        List<NhanVien> _lstnhanvien;


        public NhapKhoController(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
            _lstsach = new List<Sach>();
            _lstsachCT = new List<SachCT>();
            _lstnhaXuatBans = new List<NhaXuatBan>();
            _lsttheLoais = new List<TheLoai>();
            _lsttacGias = new List<TacGia>();
            _lstKho = new List<Kho>();
            _lstnhanvien = new List<NhanVien>();
            _lstsach = _dbcontext.Sachs.ToList();
            _lstsachCT = _dbcontext.SachCTs.ToList();
            _lsttacGias = _dbcontext.TacGias.ToList();
            _lsttheLoais = _dbcontext.TheLoais.ToList();
            _lstnhaXuatBans = _dbcontext.NhaXuatBans.ToList();
            _lstKho = _dbcontext.Khos.ToList();
            _lstnhanvien = _dbcontext.NhanViens.ToList();
            sanPhamviews();
        }

        public List<SanPhamview> sanPhamviews()
        {
            _lstsach = _dbcontext.Sachs.ToList();
            _lstsachCT = _dbcontext.SachCTs.ToList();
            _lsttacGias=_dbcontext.TacGias.ToList();
            _lsttheLoais = _dbcontext.TheLoais.ToList();
            _lstnhaXuatBans = _dbcontext.NhaXuatBans.ToList();
            _lstKho = _dbcontext.Khos.ToList();
            _lstnhanvien= _dbcontext.NhanViens.ToList();


            _lstsanPhamviews = (from a in _lstsach
                                join b in _lstnhaXuatBans on a.MaNXB equals b.ID_NXB
                                join c in _lstsachCT on a.ID_Sach equals c.MaSach
                                join d in _lsttheLoais on c.MaTheLoai equals d.ID_TheLoai
                                join e in _lsttacGias on c.MaTacGia equals e.ID_TacGia
                                join g in _lstKho on a.ID_Sach equals g.MaSach
                                join h in _lstnhanvien on g.MaNhanVien equals h.ID_NhanVien
                                select new SanPhamview()
                                { 
                                    sach=a,
                                    nhaXuatBan=b,
                                    sachCT=c,
                                    theLoai=d,
                                    tacGia=e,
                                    kho=g,
                                    nhanVien=h
                                }).ToList();

            return _lstsanPhamviews;
        }
      
        public IActionResult nhapkho( string nxb , string theongay, string theothang, string theonam, DateTime timerangefrom, DateTime rimerangeto)
        {
            if (nxb !=null)
            {
                sanPhamviews();
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.sach.TenSach.ToLower().Contains(nxb) && c.kho.NgayNhap.Month==DateTime.Now.Month && c.kho.NgayNhap.Year==DateTime.Now.Year).ToList();
                return View(_lstsanPhamviews);
            }
             if(theongay!=null)
            {
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.kho.NgayNhap.Day == DateTime.Now.Day).ToList();
                return View(_lstsanPhamviews);
            }
            if (theothang != null)
            {
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.kho.NgayNhap.Month == DateTime.Now.Month).ToList();
                return View(_lstsanPhamviews);
            }
            if (theonam != null)
            {
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.kho.NgayNhap.Year == DateTime.Now.Year).ToList();
                return View(_lstsanPhamviews);
            }
            if (timerangefrom.Year!=1 && rimerangeto.Year!=1)
            {

             _lstsanPhamviews = _lstsanPhamviews.Where(x => x.kho.NgayNhap >= timerangefrom && x.kho.NgayNhap <= rimerangeto).ToList();
                return View(_lstsanPhamviews);
            }
            else
            {
              
                return View(_lstsanPhamviews.Where(c=> c.kho.NgayNhap.Month == DateTime.Now.Month && c.kho.NgayNhap.Year == DateTime.Now.Year).ToList());
            }


          
          
                
        }
        public IActionResult chuanhap(string nxb)
        {

            if (nxb != null)
            {
                sanPhamviews();
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.sach.TenSach.ToLower().Contains(nxb)).ToList();
                return View(_lstsanPhamviews);
            }
            else
            {
                return View(_lstsanPhamviews);
            }


        }
        public IActionResult tongnhap(string nxb, string theongay, string theothang, string theonam, DateTime timerangefrom, DateTime rimerangeto)
        {


            if (nxb != null)
            {
                sanPhamviews();
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.sach.TenSach.ToLower().Contains(nxb)).ToList();
                return View(_lstsanPhamviews);
            }
            if (theongay != null)
            {
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.kho.NgayNhap.Day == DateTime.Now.Day).ToList();
                return View(_lstsanPhamviews);
            }
            if (theothang != null)
            {
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.kho.NgayNhap.Month == DateTime.Now.Month).ToList();
                return View(_lstsanPhamviews);
            }
            if (theonam != null)
            {
                _lstsanPhamviews = _lstsanPhamviews.Where(c => c.kho.NgayNhap.Year == DateTime.Now.Year).ToList();
                return View(_lstsanPhamviews);
            }
            if (timerangefrom.Year != 1 && rimerangeto.Year != 1)
            {

                _lstsanPhamviews = _lstsanPhamviews.Where(x => x.kho.NgayNhap >= timerangefrom && x.kho.NgayNhap <= rimerangeto).ToList();
                return View(_lstsanPhamviews);
            }
            else
            {
                return View(_lstsanPhamviews);
            }


        }


        public IActionResult xacnhannhap(string id)
        {
            var kho = _lstKho.FirstOrDefault(c => c.ID_Kho == id);
            kho.TrangThai = 1;
            _dbcontext.Khos.Update(kho);
            _dbcontext.SaveChanges();
            return RedirectToAction("chuanhap");
        }


        public  bool IsInTimeRange(DateTime date,DateTime timerangefrom , DateTime rimerangeto)
        {
             TimeSpan time=date.TimeOfDay, time1from = rimerangeto.TimeOfDay, time1to= timerangefrom.TimeOfDay;
            // if the time from is smaller than the time to, just filter by range
            if (time1from <= time1to)
            {
                return time >=time1from && time <=time1to;
            }
            // time from is greater than time to so two time intervals have to be created: one {timeFrom-12AM) and another one {12AM to timeTo}
            TimeSpan t2From = TimeSpan.MinValue, t2To = time1to;
            time1to = TimeSpan.MaxValue;

            return (time >= time1from && time <= time1to) || (time >= t2From && time <= t2To);
        }
    }
}
