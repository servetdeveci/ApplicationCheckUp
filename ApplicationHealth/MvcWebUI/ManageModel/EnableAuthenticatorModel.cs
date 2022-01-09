﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationHealth.MvcWebUI.AccountModel.ManageModel
{
   public class EnableAuthenticatorModel
   {
      public string SharedKey { get; set; }

      public string AuthenticatorUri { get; set; }

      public string[] RecoveryCodes { get; set; }

      public string StatusMessage { get; set; }

      public InputModel Input { get; set; }

      public class InputModel
      {
         [Required]
         [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
         [DataType(DataType.Text)]
         [Display(Name = "Verification Code")]
         public string Code { get; set; }
      }
   }
}
