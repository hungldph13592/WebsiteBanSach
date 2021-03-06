using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("TacGia")]
    public class TacGia
    {
        [Key]
        public string ID_TacGia { get; set; }
        public string HoVaTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string QueQuan { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
        public virtual ICollection<SachCT>? SachCTs { get; set; }
    }
}
