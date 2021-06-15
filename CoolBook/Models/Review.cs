using System;
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter title"), StringLength(40)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Body { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, Range(0, 5)]
        public int Rate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
