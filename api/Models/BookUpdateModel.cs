using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class BookUpdateModel : BookModel
    {
        [Required]
        public int Id { get; set; }
    }
}
