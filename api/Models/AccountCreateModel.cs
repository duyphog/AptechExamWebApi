using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class AccountCreateModel
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string[] Roles { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
