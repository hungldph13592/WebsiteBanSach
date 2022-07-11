using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("TheLoai")]
    public class TheLoai
    {
        [Key]
        public string ID_TheLoai { get; set; }
        public string TenTL { get; set; }
        public virtual ICollection<SachCT>? SachCTs { get; set; }
    }
}
