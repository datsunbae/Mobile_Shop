using System.ComponentModel.DataAnnotations;

namespace Phone_Ecommerce_Manage.Areas.Admin.Models
{
    public class AdminLoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = ("Vui lòng nhập tài khoản"))]
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; }
    }
}
