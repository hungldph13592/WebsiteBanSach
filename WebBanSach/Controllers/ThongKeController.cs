using Microsoft.AspNetCore.Mvc;
using WebBanSach.ModelGhep;
using WebBanSach.Models;
using WebBanSach.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace WebBanSach.Controllers
{
    [Authorize(Policy = "StaffOnly")]
    public class ThongKeController : Controller
    {
        public dbcontext _dbcontext;
        List<KhachHang> _lstKhachHang;
        List<HoaDon> _lstHoaDon;
        List<HoaDonCT> _lstHoaDonCT;
        List<Sach> _lstSach;
        List<Thongke> _lstThongKe;

        public ThongKeController(dbcontext dbcontext)
        {
            this._dbcontext = dbcontext;
            _lstKhachHang = new List<KhachHang>();
            _lstHoaDon = new List<HoaDon>();
            _lstHoaDonCT = new List<HoaDonCT>();
            _lstSach = new List<Sach>();
            thongkes();
        }

        public List<Thongke> thongkes()
        {
            _lstKhachHang = _dbcontext.KhachHangs.ToList();
            _lstHoaDon = _dbcontext.HoaDons.ToList();
            _lstHoaDonCT = _dbcontext.HoaDonCTs.ToList();
            _lstSach = _dbcontext.Sachs.ToList();

            _lstThongKe = (from a in _lstKhachHang
                           join b in _lstHoaDon on a.ID_KhachHang equals b.MaKH
                           join c in _lstHoaDonCT on b.ID_HoaDon equals c.MaHoaDon
                           join d in _lstSach on c.MaSach equals d.ID_Sach
                           select new Thongke()
                           {
                               khachHang = a,
                               hoaDon = b,
                               hoaDonCT = c,
                               sach = d
                           }).ToList();

            return _lstThongKe;
        }
        public IActionResult closedEx()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Tên Khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Ngày bán";
            worksheet.Cell(currentRow, 4).Value = "Tổng tiền";


            foreach (var x in _lstThongKe)
            {
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = x.khachHang.HoVaTen;
                worksheet.Cell(currentRow, 3).Value = x.hoaDon.NgayMua;
                worksheet.Cell(currentRow, 4).Value = x.hoaDon.TongTien;

            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString() + ".xlsx");
        }
        public IActionResult closedExday()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Tên Khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Ngày bán";
            worksheet.Cell(currentRow, 4).Value = "Tổng tiền";


            foreach (var x in _lstThongKe.Where(c => c.hoaDon.NgayMua.Day == DateTime.Now.Day && c.hoaDon.NgayMua.Month== DateTime.Now.Month && c.hoaDon.NgayMua.Year == DateTime.Now.Year))
            {
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = x.khachHang.HoVaTen;
                worksheet.Cell(currentRow, 3).Value = x.hoaDon.NgayMua;
                worksheet.Cell(currentRow, 4).Value = x.hoaDon.TongTien;

            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString() + ".xlsx");
        }
        public IActionResult closedExmonth()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Tên Khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Ngày bán";
            worksheet.Cell(currentRow, 4).Value = "Tổng tiền";


            foreach (var x in _lstThongKe.Where(c => c.hoaDon.NgayMua.Month == DateTime.Now.Month &&  c.hoaDon.NgayMua.Day == DateTime.Now.Day))
            {
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = x.khachHang.HoVaTen;
                worksheet.Cell(currentRow, 3).Value = x.hoaDon.NgayMua;
                worksheet.Cell(currentRow, 4).Value = x.hoaDon.TongTien;

            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString() + ".xlsx");
        }
        public IActionResult closedExyear()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Tên Khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Ngày bán";
            worksheet.Cell(currentRow, 4).Value = "Tổng tiền";


            foreach (var x in _lstThongKe.Where(c => c.hoaDon.NgayMua.Year == DateTime.Now.Year))
            {
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = x.khachHang.HoVaTen;
                worksheet.Cell(currentRow, 3).Value = x.hoaDon.NgayMua;
                worksheet.Cell(currentRow, 4).Value = x.hoaDon.TongTien;

            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString() + ".xlsx");
        }

        public IActionResult Index(string nxb, string theongay, string theothang, string theonam, DateTime timerangefrom, DateTime rimerangeto)
        {
            thongkes();
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var res = Enumerable
                .Range(0, 1 + (end - start).Days)
                .Select(x => start.AddDays(x))
                .GroupJoin(_lstThongKe,
                    dt => dt, o => o.hoaDon.NgayMua.Date,

                    (dt, orders) => new OrderDateSummary { Date = dt, Total = orders.Sum(c => c.hoaDon.TongTien) }).ToList();
            //   .ToDictionary(x => x.Date, x => x.Total);

            ViewBag.Bieudo = res;
            if (nxb != null)
            {
                thongkes();
                _lstThongKe = _lstThongKe.Where(c => c.sach.TenSach.ToLower().Contains(nxb)).ToList();
                return View(_lstThongKe);

            }
            if (theongay != null)
            {
                _lstThongKe = _lstThongKe.Where(c => c.hoaDon.NgayMua.Day == DateTime.Now.Day).ToList();
                return View(_lstThongKe);
            }
            if (theothang != null)
            {
                _lstThongKe = _lstThongKe.Where(c => c.hoaDon.NgayMua.Month == DateTime.Now.Month).ToList();
                return View(_lstThongKe);
            }
            if (theonam != null)
            {
                _lstThongKe = _lstThongKe.Where(c => c.hoaDon.NgayMua.Year == DateTime.Now.Year).ToList();
                return View(_lstThongKe);
            }
            if (timerangefrom.Year != 1 && rimerangeto.Year != 1)
            {

                _lstThongKe = _lstThongKe.Where(x => x.hoaDon.NgayMua >= timerangefrom && x.hoaDon.NgayMua <= rimerangeto).ToList();
                return View(_lstThongKe);
            }
            else
            {
                return View(_lstThongKe);
            }
        }
        public IActionResult Detail(string Id)
        {
            _lstThongKe = _lstThongKe.Where(c => c.hoaDonCT.MaHoaDon == Id).ToList();
            return View(_lstThongKe);
        }
    }
}
