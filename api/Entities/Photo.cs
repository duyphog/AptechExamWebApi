using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
