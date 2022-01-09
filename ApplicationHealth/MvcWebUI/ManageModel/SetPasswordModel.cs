using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel.ManageModel
{
    public class SetPasswordModel
   {
      public InputModel Input { get; set; }
      public string StatusMessage { get; set; }

      public class InputModel
      {
         [Required]
         [StringLength(100, ErrorMessage = "{0} en az {2} en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 8)]
         [DataType(DataType.Password)]
         [Display(Name = "Yeni Şifre")]
         public string NewPassword { get; set; }

         [DataType(DataType.Password)]
         [Display(Name = "Yeni Şifre Doğrula")]
         [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor.")]
         public string ConfirmPassword { get; set; }
      }
   }
}
