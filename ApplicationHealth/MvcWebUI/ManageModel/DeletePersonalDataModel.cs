using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationHealth.MvcWebUI.AccountModel.ManageModel
{
   public class DeletePersonalDataModel
   {
      public InputModel Input { get; set; }
      public bool RequirePassword { get; set; }

      public class InputModel
      {
         [Required]
         [DataType(DataType.Password)]
         [Display(Name = "Şifre")]
         public string Password { get; set; }
      }

   }
}
