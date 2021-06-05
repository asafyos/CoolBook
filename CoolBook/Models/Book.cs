using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        public int Name { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        
        public int Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public String ImageUrl { get; set; }

        public List<Category> Categories { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
