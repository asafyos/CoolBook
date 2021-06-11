using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Must enter book name"), StringLength(100)]
        public int Name { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }


        [Required, DataType(DataType.Currency), Range(0, 1000)]
        public double Price { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }


        [DataType(DataType.ImageUrl), Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public List<Category> Categories { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
