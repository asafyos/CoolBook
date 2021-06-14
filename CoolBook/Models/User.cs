using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public enum UserRole
    {
        Admin,
        Manager,
        Client
    }

    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter user name"), StringLength(20), Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Must enter full name"), StringLength(40), Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Must enter email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must enter password"), DataType(DataType.Password)]
        public string Password { get; set; }

        public Gender Gender { get; set; }

        public UserRole Role { get; set; } = UserRole.Client;
    }
}
