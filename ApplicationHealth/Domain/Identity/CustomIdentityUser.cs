using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.Domain.Identity
{

    public class CustomIdentityUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
       

    }
}
