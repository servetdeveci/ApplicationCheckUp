using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class LoginModel
    {
        public string ReturnUrl { get; set; } = "/";

        public string ErrorMessage { get; set; } = "";

        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        [StringLength(15, ErrorMessage = "Kullanıcı adı en fazla {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
