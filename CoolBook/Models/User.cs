using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter user name"), StringLength(20), Display(Name = "User Name")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Must enter full name"), StringLength(40), Display(Name = "Full Name")]
        public String FullName { get; set; }

        [Required(ErrorMessage = "Must enter email"), DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Must enter password"), DataType(DataType.Password)]
        public String Password { get; set; }

        public Gender Gender { get; set; }
    }
}
