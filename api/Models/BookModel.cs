using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class BookModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Descriptions { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string Publisher { get; set; }

        public string Language { get; set; }

        public int? Pages { get; set; }

        [Required]
        public string CategoryCode { get; set; }

        public int[] Authors  { get; set; }
    }
}
