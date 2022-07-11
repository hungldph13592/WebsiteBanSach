using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanSach.Data;
using WebBanSach.Helpers;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    [Authorize(Policy = "NoStaffAllowed")]
    public class GioHangsController : Controller
    {

        private readonly dbcontext _context;
        public GioHangsController(dbcontext context)
        {
            _context = context;
        }
        public List<GioHang> sp
        {
            get
            {
                var data = HttpContext.Session.Get<List<GioHang>>("giohang");
                if (data == null)
                {

                    data = new List<GioHang>();
                }
                return data;
            }
        }
        public List<GioHang> Giohangs
        {
            get
            {
                var data = HttpContext.Session.Get<List<GioHang>>("giohang");
                if (data == null)
                {

                    data = new List<GioHang>();
                }
                return data;
            }
        }
        public List<Sach> sanPhams
        {
            get
            {
                var data = _context.Sachs.ToList();
                if (data == null)
                {

                    data = new List<Sach>();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            ViewData["username"] = HttpContext.Session.GetString("username");

            ViewData["dbsanpham"] = sanPhams;
            return View(Giohangs);
        }
        public IActionResult AddToCart(string id, int SoLuong)
        {

            Sach sanPham = _context.Sachs.SingleOrDefault(c => c.ID_Sach == id);

            var myCart = Giohangs;
            var item = myCart.SingleOrDefault(c => c.IdSanPham == id);

            if (item == null)
            {

                item = new GioHang
                {
                    IdSanPham = id,
                    TenSanPham = sanPham.TenSach,
                    DonGia = sanPham.Gia,
                    SoLuong = SoLuong,
                    Hinhanh = sanPham.HinhAnh,
                };
                myCart.Add(item);



            }
            else if (SoLuong > 2)
            {
                item.SoLuong += SoLuong;

            }
            else
            {
                item.SoLuong++;
            }
            HttpContext.Session.Set("giohang", myCart);

            return RedirectToAction("Index");
        }
        public IActionResult SuaSoLuong(string id, int soluongmoi)
        {
            Sach sanPham = _context.Sachs.SingleOrDefault(c => c.ID_Sach == id);
            var myCart = Giohangs;
            var item = myCart.SingleOrDefault(c => c.IdSanPham == id);

            if (item != null)
            {

                item.SoLuong = soluongmoi;
            }
            HttpContext.Session.Set("giohang", myCart);
            return RedirectToAction("Index");


        }

        public IActionResult XoaKhoiGio(string id)
        {
            var myCart = Giohangs;
            var item = myCart.SingleOrDefault(c => c.IdSanPham == id);
            if (item != null)
            {
                myCart.Remove(item);
            }
            HttpContext.Session.Set("giohang", myCart);
            return RedirectToAction("Index");
        }
      
    }
}
