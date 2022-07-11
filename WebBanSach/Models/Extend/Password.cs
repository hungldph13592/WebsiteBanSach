using System.ComponentModel.DataAnnotations;

namespace WebBanSach.Models.Extend
{
    public class Password
    {
        [MinLength(5)]
        [MaxLength(20)]
        [Required]
        public string pw { get; set; }
    }
}
