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
        public string Name { get; set; }
        public double Lontitude { get; set; }
        public double Latitude { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
