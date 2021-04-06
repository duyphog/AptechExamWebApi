using System;
using System.Collections.Generic;
using api.Entities;

namespace api.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Introduction { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; } = DateTime.Now;

        public virtual ICollection<string> Roles { get; set; }
    }
}
