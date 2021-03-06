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

        public Gender Gender { get; set; }

        [Display(Name = "Birth Date"), DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        [Display(Name = "Phone Number"), 
            DataType(DataType.PhoneNumber), 
            RegularExpression(@"^\(?([0-9]{2,3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", 
                              ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
    }
}
