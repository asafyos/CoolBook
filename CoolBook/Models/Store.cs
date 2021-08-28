using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class Store
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Must enter store name"), StringLength(40)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Provide store location lontidue")]
        public double Lontitude { get; set; }

        [Required(ErrorMessage = "Provide store location latidue")]
        public double Latitude { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
