using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class ResetPasswordModel
   {
      public InputModel Input { get; set; }

      public class InputModel
      {
         [Required]
         [EmailAddress]
         public string Email { get; set; }

         [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
         [StringLength(100, ErrorMessage = "Şifre alanı en az {2} karakterden oluşmalıdır", MinimumLength = 8)]
         [DataType(DataType.Password, ErrorMessage = "Şifre uygun değil. Büyük harf, küçük harf, sayı ve noktalama kullanılmalıdır.")]
         [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Şifre en az 8 karakter olmalı ve en az 1 büyük ve küçük harf, sayı ve noktalama içermelidir.")]
         [Display(Name = "Yeni Parola")]
         public string Password { get; set; }

         [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
         [StringLength(100, ErrorMessage = "Şifre alanı en az {2} karakterden oluşmalıdır", MinimumLength = 8)]
         [DataType(DataType.Password, ErrorMessage = "Şifre uygun değil. Büyük harf, küçük harf, sayı ve noktalama kullanılmalıdır.")]
         [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Şifre en az 8 karakter olmalı ve en az 1 büyük ve küçük harf, sayı ve noktalama içermelidir.")]
         [Display(Name = "Yeni Parolayı Doğrula")]
         [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor.")]
         public string ConfirmPassword { get; set; }

         public string Code { get; set; }
      }
   }
}
