using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel.ManageModel
{
    public class ChangePasswordModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bu Alan Gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut parola")]
        public string OldPassword { get; set; }

      [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
      [StringLength(100, ErrorMessage = "Şifre alanı en az {2} karakterden oluşmalıdır", MinimumLength = 8)]
      [DataType(DataType.Password, ErrorMessage = "Şifre uygun değil. Büyük harf, küçük harf, sayı ve noktalama kullanılmalıdır.")]
      [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Şifre en az 8 karakter olmalı ve en az 1 büyük ve küçük harf, sayı ve noktalama içermelidir.")]
      [Display(Name = "Yeni Parola")]
      public string NewPassword { get; set; }

      [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
      [StringLength(100, ErrorMessage = "Şifre alanı en az {2} karakterden oluşmalıdır", MinimumLength = 8)]
      [DataType(DataType.Password, ErrorMessage = "Şifre uygun değil. Büyük harf, küçük harf, sayı ve noktalama kullanılmalıdır.")]
      [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Şifre en az 8 karakter olmalı ve en az 1 büyük ve küçük harf, sayı ve noktalama içermelidir.")]
      [Display(Name = "Yeni Parolayı Doğrula")]
      public string ConfirmPassword { get; set; }

    }
}
