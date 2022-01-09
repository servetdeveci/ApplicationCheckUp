using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel.ManageModel
{
    public class IndexModel
   {
      [Display(Name = "Kullanıcı Adı")]
      public string Username { get; set; }

      public bool IsEmailConfirmed { get; set; }

      public string StatusMessage { get; set; }

      public InputModel Input { get; set; }

      public class InputModel
      {
         [Required(ErrorMessage = "Bu Alan Gereklidir.")]
         [EmailAddress]
         [Display(Name = "E-Posta Adresi")]
         public string Email { get; set; }

         [Phone]
         [Display(Name = "Telefon Numarası")]
         public string PhoneNumber { get; set; }
      }
   }
}
