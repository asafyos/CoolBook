using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter author name"), StringLength(40)]
        public string Name { get; set; }

        [Required, Display(Name = "Birth Date"), DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [StringLength(40)]
        public string Country { get; set; }
        
        public List<Book> Books { get; set; }
    }
}
