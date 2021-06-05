using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter author name"), StringLength(40)]
        public String Name { get; set; }

        [Required, Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public String Country { get; set; }
        
        public List<Book> Books { get; set; }
    }
}
