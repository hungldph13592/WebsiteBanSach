using WebBanSach.Models;
namespace WebBanSach.ModelGhep

{
    public class SanPhamview
    {
        public NhaXuatBan nhaXuatBan { get; set; }
        public TheLoai theLoai { get; set; }
        public TacGia tacGia { get; set; }
        public SachCT sachCT { get; set; }
        public Sach sach { get; set; }
        public Kho kho { get; set; }
        public NhanVien nhanVien { get; set; }
    }
}
