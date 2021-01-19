using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personal.Finance.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, ErrorMessage = "First name can't be longer than 30 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(30, ErrorMessage = "Last name can't be longer than 30 characters")]
        public string LastName { get; set; }

        public DateTime Created { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
