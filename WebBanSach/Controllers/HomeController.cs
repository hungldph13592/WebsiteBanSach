using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System.Diagnostics;
using System.Security.Claims;
using WebBanSach.Data;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbcontext _DB;
        public HomeController(ILogger<HomeController> logger, dbcontext context)
        {
            _logger = logger;
            _DB = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> KhachHang()
        {
            var khachhang = _DB.KhachHangs.Where(c => c.Email == User.Identity.Name).FirstOrDefault();

            return View(khachhang);
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KhachHang([Bind("ID_KhachHang,Email,HoVaTen,SDT,DiaChi,NgaySinh,MatKhau,TrangThai")] KhachHang khachHang)
        {
            if (khachHang != null)
            {         
                _DB.Update(khachHang);
                await _DB.SaveChangesAsync();
                ViewBag.ThongBao = "Sửa thông tin thành công !";
            }
           
            
            return View(khachHang);
        }
        public ActionResult TheLoai(string idTl)
        {
            List<SachCT> FiterTL = new List<SachCT>();
            FiterTL = _DB.SachCTs.Where(c => c.MaTheLoai == idTl).ToList();//lọc theo thể loại
            List<Sach> lstSach = new List<Sach>();
            foreach (var item in FiterTL)
            {
                Sach Sachs = new Sach();
                Sachs = _DB.Sachs.First(c => c.ID_Sach == item.MaSach);
                lstSach.Add(Sachs);
            }
            ViewBag.NXB = _DB.NhaXuatBans.ToList();
            ViewBag.SachTL = lstSach;
            ViewBag.TheLoai = _DB.TheLoais.ToList();
            return View(FiterTL);
        }
        public IActionResult TrangChu(string currentFilter, string NameBook, int? page)
        {
            List<TheLoai> lstTl = new List<TheLoai>();
            List<Sach> LstSeachbook = new List<Sach>();
            var lstProdust = _DB.Sachs.ToList().Where(c=> c.TrangThai == 1);// List ALL sách
            //FiterTL = _DB.SachCTs.Where(c => c.MaTheLoai == idTL).ToList();//lọc theo thể loại
            LstSeachbook = _DB.Sachs.Where(c => c.TenSach.Contains(NameBook)).ToList();// Tìm kiếm theo tên sách
            lstTl = _DB.TheLoais.ToList();// list thể loại
            List<Sach> lst = new List<Sach>();
            int pagesize = 4;
            int PageNumber = (page ?? 1);
            ViewBag.TheLoai = lstTl;

            if (NameBook != null)
            {
                page = 1;
            }
            else
            {
                NameBook = currentFilter;
            }
            if (!string.IsNullOrEmpty(NameBook))
            {
                return View(LstSeachbook.ToPagedList(PageNumber, pagesize));
            }
            ViewBag.currentFilter = NameBook;
            ViewBag.NXB = _DB.NhaXuatBans.ToList();
            return View(lstProdust.ToPagedList(PageNumber, pagesize));
        }
        public IActionResult DetailBook(string id)
        {
            List<SachCT> lstSachCT = new List<SachCT>();
            lstSachCT = _DB.SachCTs.Where(c => c.MaSach == id).ToList();
            List<TacGia> lstTacGia = new List<TacGia>();
            List<TheLoai> lstTheLoai = new List<TheLoai>();
            foreach (var item in lstSachCT)
            {
                TacGia tacGia = new TacGia();
                tacGia = _DB.TacGias.First(c => c.ID_TacGia == item.MaTacGia);
                lstTacGia.Add(tacGia);
                TheLoai theLoai = new TheLoai();
                theLoai = _DB.TheLoais.First(c=>c.ID_TheLoai == item.MaTheLoai);
                lstTheLoai.Add(theLoai);
            }
            ViewBag.NXB = _DB.NhaXuatBans.ToList();
            ViewBag.TacGia = lstTacGia;
            ViewBag.TheLoai = lstTheLoai;
            var DetailSP = _DB.Sachs.Where(c => c.ID_Sach == id).FirstOrDefault();
            var DetailSPCT = _DB.SachCTs.Where(c => c.MaSach == id).FirstOrDefault();  
            return View(DetailSP);
        }

        public IActionResult Qllogin()
        {
            return RedirectToAction("Login", "Login");
        }

        [Authorize(Policy ="NoStaffAllowed")]
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}