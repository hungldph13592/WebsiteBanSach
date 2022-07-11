using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public string ID_HoaDon { get; set; }
        [ForeignKey("KhachHang")]
        public string MaKH { get; set; }
        public DateTime NgayMua { get; set; }
        public double TongTien { get; set; }
        [Range(0, 2)]
        public int TrangThai { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
