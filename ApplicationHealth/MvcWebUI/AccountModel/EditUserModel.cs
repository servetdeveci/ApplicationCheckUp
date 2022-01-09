using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApplicationHealth.Domain.Identity;

namespace ApplicationHealth.MvcWebUI.AccountModel
{
    public class EditUserModel
    {
        public string UserId { get; set; }
        public List<CustomIdentityRole> IdentityRoles { get; set; }
        public CustomIdentityUser EditUser { get; set; }
        public IList<string> UserRoles { get; set; }

        [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
        [EmailAddress(ErrorMessage = "'{0}' alanı gereklidir.")]
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

        [Required(ErrorMessage = "'{0}' alanı gereklidir.")]
        public List<CustomIdentityRole> Roles { get; set; }
      


    }
}
