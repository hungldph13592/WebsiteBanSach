using System.ComponentModel.DataAnnotations;

namespace WebBanSach.Models.Extend
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Hay nhap username")]
        public string User { get; set; }
        [Required(ErrorMessage = "Hay nhap password")]
        public string Password { get; set; }
    }
}
