using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class User
    {
        public int Id { get; set; }

        public String UserName { get; set; }

        public String FullName { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public Gender Gender { get; set; }
    }
}
