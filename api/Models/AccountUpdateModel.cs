using System;
namespace api.Models
{
    public class AccountUpdateModel
    {
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Introduction { get; set; }

        public string Email { get; set; }
    }
}
