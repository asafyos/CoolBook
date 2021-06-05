using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class Category
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String ImageUrl { get; set; }

        public List<Book> Books { get; set; }
    }
}
