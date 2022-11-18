using System.ComponentModel.DataAnnotations;

namespace Phone_Ecommerce_Manage.ModelViews
{
    public class RegisterViewModel
    {
        [Key]
        public int IdCustomer { get; set; }

        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        public string NameCustomer { get; set; }


        [MaxLength(50)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Vui lòng nhập Username")]
        [MinLength(6, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 6 ký tự")]
        public string Username { get; set; }

        [MaxLength(150)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]
        public string Email { get; set; }

        [MaxLength(11)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Sai định dạng điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

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
