using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class ForgotPasswordModel
   {
      [Required(ErrorMessage = "E-posta Adresi alanı gereklidir")]
      [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi olmalıdır")]
      public string Email { get; set; }
   }
}
