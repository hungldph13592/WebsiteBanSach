using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanSach.Data;
using WebBanSach.Helpers;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    [Authorize(Policy = "NoStaffAllowed")]
    public class PaymentController : Controller
    {
        private readonly dbcontext _context;


        public PaymentController(dbcontext Context)
        {
            _context = Context;
        }
        public List<GioHang> Giohangs
        {
            get
            {
                var data = HttpContext.Session.Get<List<GioHang>>("giohang");

                return data;
            }
        }
        
        public IActionResult Index()
        {
            
                ViewBag.Carts = Giohangs;
                ViewBag.TongTien = Giohangs.Sum(c => c.ThanhTien);
                ViewBag.NguoiDung = _context.KhachHangs.Where(c => c.Email == User.Identity.Name).FirstOrDefault();

           

            return View();
        }
        public IActionResult PaymentBook(KhachHang khachHang)
        {
            var NguoiDung = _context.KhachHangs.Where(c => c.Email == User.Identity.Name).FirstOrDefault();


            if (khachHang == null || Giohangs == null)
            {
                ViewBag.MessError = "Thanh toán thất bại";
            }
            else
            {

                var lstCart = Giohangs;

                List<Sach> lstSanPham = _context.Sachs.ToList();

                for (int i = 0; i < lstSanPham.Count; i++)
                {
                    for (int j = 0; j < lstCart.Count; j++)
                    {
                        if (lstSanPham[i].ID_Sach == lstCart[j].IdSanPham)
                        {

                            lstSanPham[i].SoLuong = lstSanPham[i].SoLuong - lstCart[j].SoLuong;

                            HttpContext.Session.Remove("giohang");
                            _context.Sachs.UpdateRange(lstSanPham);
                            _context.SaveChanges();


                        }
                    }

                }
                foreach (var item in lstCart)
                {
                    HoaDon hoaDon = new HoaDon();
                    hoaDon.ID_HoaDon = Guid.NewGuid().ToString();
                    hoaDon.MaKH = NguoiDung.ID_KhachHang;
                    hoaDon.TongTien = item.ThanhTien;
                    hoaDon.NgayMua = DateTime.Now;
                    hoaDon.TrangThai = 0;
                    _context.HoaDons.Add(hoaDon);


                    List<HoaDonCT> listHoaDonCTs = new List<HoaDonCT>();
                    HoaDonCT hoaDonCT = new HoaDonCT();
                    hoaDonCT.ID_HDCT = Guid.NewGuid().ToString();
                    hoaDonCT.MaSach = item.IdSanPham;
                    hoaDonCT.MaHoaDon = hoaDon.ID_HoaDon;
                    hoaDonCT.SoLuong = item.SoLuong;

                    listHoaDonCTs.Add(hoaDonCT);
                    _context.HoaDonCTs.AddRange(listHoaDonCTs);
                    _context.SaveChanges();
                    ViewBag.SuccessMessage = "Thanh toán thành công";

                }
               
            }
            return View();
        }
    }
}

