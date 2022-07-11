using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    public class Kho
    {
        [Key]
        public string ID_Kho { get; set; }
        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayNhap { get; set; }
        public double GiaNhap { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        public virtual Sach Sach { get; set; }
    }
}
