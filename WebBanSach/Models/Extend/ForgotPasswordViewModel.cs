using System.ComponentModel.DataAnnotations;

namespace WebBanSach.Models.Extend
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Mời bạn nhập Email đã đăng ký")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
