using System;
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Must enter full name"), StringLength(40), Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Must enter date of birth"), Display(Name = "Birth Date"), DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Must enter Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Must enter phone number"), Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
