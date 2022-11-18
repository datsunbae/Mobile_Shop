using System.ComponentModel.DataAnnotations;

namespace Phone_Ecommerce_Manage.ModelViews
{
    public class ForgetPasswordViewModel
    {

        [MaxLength(150)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]
        public string Email { get; set; }
    }
}
