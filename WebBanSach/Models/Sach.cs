using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("Sach")]
    public class Sach
    {
        [Key]
        [Required]
        public string ID_Sach { get; set; }
        [ForeignKey("NhaXuatBan")]
        public string MaNXB { get; set; }

        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        [Range(1, 1000000)]
        public int SoTrang { get; set; }
        [Range(0, 100)]
        public int TaiBan { get; set; }
        [Range(1,999999)]
        public int Gia { get; set; }
        public DateTime NgayNhap { get; set; }
        public int SoLuong { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
        public NhaXuatBan? NhaXuatBan { get; set; }
        public virtual ICollection<SachCT>? SachCTs { get; set; }
        public virtual ICollection<HoaDonCT>? HoaDonCTs { get; set; }
        public virtual ICollection<Kho>? Khos { get; set; }
    }
}
