using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Models
{
    public class Author
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public String Country { get; set; }
        
        public List<Book> Books { get; set; }
    }
}
