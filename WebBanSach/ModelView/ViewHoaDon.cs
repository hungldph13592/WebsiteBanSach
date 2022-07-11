using WebBanSach.Models;

namespace WebBanSach.ModelView
{
    public class ViewHoaDon
    {
        public KhachHang khachHang { get; set; }
        public Sach sach { get; set; }
        public HoaDon hoaDon { get; set; }
        public HoaDonCT hoaDonCT { get; set; }
        public NhaXuatBan nhaXuatBan { get; set; }
        public TacGia tacGia { get; set; }
        public TheLoai TheLoai { get; set; }
        public SachCT sachCT { get; set; }
        public ViewHoaDon()
        {
            khachHang = new KhachHang();
            sach = new Sach();
            hoaDon = new HoaDon();
            hoaDonCT = new HoaDonCT();
            nhaXuatBan = new NhaXuatBan();
            TheLoai = new TheLoai();
            tacGia = new TacGia();
            sachCT = new SachCT();
        }

        public ViewHoaDon(KhachHang khachHang, Sach sach, HoaDon hoaDon, HoaDonCT hoaDonCT, NhaXuatBan nhaXuatBan, TacGia tacGia, TheLoai theLoai, SachCT sachCT)
        {
            this.khachHang = khachHang;
            this.sach = sach;
            this.hoaDon = hoaDon;
            this.hoaDonCT = hoaDonCT;
            this.nhaXuatBan = nhaXuatBan;
            this.tacGia = tacGia;
            TheLoai = theLoai;
            this.sachCT = sachCT;
        }
    }
}
