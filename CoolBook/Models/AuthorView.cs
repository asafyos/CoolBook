using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Models
{
    public class AuthorView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Birth Date"), DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Country { get; set; }

        [Display(Name = "Books Count")]
        public int BookCount { get; set; }
    }
}
