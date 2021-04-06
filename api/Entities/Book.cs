using System;
using System.Collections.Generic;

namespace api.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Title  { get; set; }

        public string Descriptions { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string Publisher { get; set; }

        public string Language { get; set; }

        public int? Pages { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public string CategoryCode { get; set; }

        public Category Category { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
