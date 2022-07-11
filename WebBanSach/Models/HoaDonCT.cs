using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("HoaDonCT")]
    public class HoaDonCT
    {
        [Key]
        public string ID_HDCT { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        [ForeignKey("HoaDon")]
        public string MaHoaDon { get; set; }
        public int SoLuong { get; set; }
        public virtual HoaDon HoaDon { get; set; }
        public virtual Sach Sach { get; set; }
    }
}
