using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("SachCT")]
    public class SachCT
    {
        [Key]
        public string ID_SachCT { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        [ForeignKey("TheLoai")]
        public string MaTheLoai { get; set; }
        [ForeignKey("TacGia")]
        public string MaTacGia { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
        public virtual Sach Sach { get; set; }
        public virtual TheLoai TheLoai { get; set; }
        public virtual TacGia TacGia { get; set; }
       
    }
}
