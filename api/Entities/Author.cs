using System;
using System.Collections.Generic;

namespace api.Entities
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string EmailAddress { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
