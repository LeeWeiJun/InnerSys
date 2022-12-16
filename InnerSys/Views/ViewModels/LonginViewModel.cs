using System.ComponentModel.DataAnnotations;

namespace InnerSys.Views.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號!")]
        [Display(Name = "帳號")]
        public string UserAccount { get; set; }

        [Required(ErrorMessage = "請輸入密碼!")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}