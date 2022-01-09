using ApplicationHealth.Domain.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class RegisterModel
   {
      public RegisterViewModel RegisterView { get; set; }

      public List<CustomIdentityRole> Roles { set; get; }

      public string ReturnUrl { get; set; }

      public class RegisterViewModel
      {
         //[EmailAddress(ErrorMessage = "'{0}' alanı gereklidir.")]
         [Display(Name = "E-Posta")]
         public string Email { get; set; }

         [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
         [Display(Name = "Gerçek Adı")]
         public string Name { get; set; }

         [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
         [Display(Name = "Kullanıcı Adı")]
         public string UserName { get; set; }

         [Phone]
         [RegularExpression("^[0-9]*$", ErrorMessage = "{0} alanı nümerik olmalıdır")]
         [StringLength(15, MinimumLength = 10, ErrorMessage = "Cep Telefonu numarası en az {2} en fazla {1} karakter olmalıdır")]
         [Display(Name = "Cep Telefonu")]
         public string MobilePhone { get; set; }

         [Phone]
         [Display(Name = "Telefon")]
         [StringLength(15, MinimumLength = 10, ErrorMessage = "Telefon numarası en az {2} en fazla {1} karakter olmalıdır")]
         [RegularExpression("^[0-9]*$", ErrorMessage = "{0} alanı nümerik olmalıdır")]
         public string Phone { get; set; }
         public string Password { get; set; } = "";
         public string ConfirmPassword { get; set; } = "";

         [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
         public List<string> Roles { get; set; }

      }
   }
}
