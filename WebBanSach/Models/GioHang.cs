namespace WebBanSach.Models
{
    public class GioHang
    {
        public string Hinhanh { get; set; }
        public string IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public int ThanhTien
        {
            get
            {
                return SoLuong * DonGia;
            }
        }
    }
}
