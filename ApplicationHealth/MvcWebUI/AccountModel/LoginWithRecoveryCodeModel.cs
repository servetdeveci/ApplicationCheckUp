using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class LoginWithRecoveryCodeModel
   {
      public InputModel Input { get; set; }

      public string ReturnUrl { get; set; }

      public class InputModel
      {
         [BindProperty]
         [Required]
         [DataType(DataType.Text)]
         [Display(Name = "Recovery Code")]
         public string RecoveryCode { get; set; }
      }
   }
}
