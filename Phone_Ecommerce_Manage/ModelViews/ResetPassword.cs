using System.ComponentModel.DataAnnotations;

namespace Phone_Ecommerce_Manage.ModelViews
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
