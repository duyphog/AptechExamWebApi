using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace api.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        public string Introduction { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; } = DateTime.Now;

        public ICollection<AppUserRole> UserRole { get; set; }
    }
}
