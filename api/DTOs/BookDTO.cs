using System;
using System.Collections.Generic;

namespace api.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Descriptions { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string Publisher { get; set; }

        public string Language { get; set; }

        public int? Pages { get; set; }

        public string PhotoUrl { get; set; }

        public ICollection<PhotoDTO> Photos { get; set; }

        public CategoryDTO Category { get; set; }

        public ICollection<AuthorDTO> Author { get; set; }
    }
}
