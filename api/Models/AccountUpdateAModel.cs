using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class AccountUpdateAModel : AccountUpdateModel
    {
        [Required]
        public int Id { get; set; }
    }
}
