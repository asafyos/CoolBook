using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must enter category name"), StringLength(40)]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl), Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public List<Book> Books { get; set; }
    }
}
