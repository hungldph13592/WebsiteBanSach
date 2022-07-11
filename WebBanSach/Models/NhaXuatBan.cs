using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("NhaXuatBan")]
    public class NhaXuatBan
    {
        [Key]
        public string ID_NXB { get; set; }
        public string TenXuatBan { get; set; }
        [Range(0,1)]
        public int TrangThai { get; set; }
        public virtual ICollection<Sach>? Sachs { get; set; }
    }
}
