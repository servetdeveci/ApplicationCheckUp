﻿using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class LoginWith2faModel
   {
      public InputModel Input { get; set; }

      public bool RememberMe { get; set; }

      public string ReturnUrl { get; set; }

      public class InputModel
      {
         [Required]
         [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
         [DataType(DataType.Text)]
         [Display(Name = "Authenticator code")]
         public string TwoFactorCode { get; set; }

         [Display(Name = "Remember this machine")]
         public bool RememberMachine { get; set; }
      }
   }
}
