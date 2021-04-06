using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Category
    {
        [Key]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please add a Category name.")]
        public string Name { get; set; }

        public string Descriptions { get; set; } = null;

        public bool Status { get; set; } = true;

        public ICollection<Book> Books { get; set; }
    }
}
