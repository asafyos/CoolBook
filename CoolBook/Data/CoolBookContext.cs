using Microsoft.EntityFrameworkCore;
using CoolBook.Models;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CoolBook.Data
{
    public class BookCategories
    {
        public int Id { get; set; }
        public int[] Categories { get; set; }
    }

    public class CoolBookContext : DbContext
    {
        public CoolBookContext(DbContextOptions<CoolBookContext> options)
            : base(options)
        {
            if (Database.EnsureCreated()) InitData(this);
        }

        private void InitData(CoolBookContext coolBookContext)
        {
            var storeJson = File.ReadAllText("./DbData\\" + typeof(Store).Name + ".json");
            var storesFromJson = JsonConvert.DeserializeObject<IEnumerable<Store>>(storeJson);
            var stores = storesFromJson.Select(x => new Store
            {
                Name = x.Name,
                Lontitude = x.Lontitude,
                Latitude = x.Latitude,
                Phone = x.Phone
            });
            Store.AddRange(stores);

            var userJson = File.ReadAllText("./DbData\\" + typeof(User).Name + ".json");
            var usersFromJson = JsonConvert.DeserializeObject<IEnumerable<User>>(userJson);
            var users = usersFromJson.Select(x => new User
            {
                UserName = x.UserName,
                Email = x.Email,
                Password = x.Password,
                Role = x.Role,
                UserInfo = new UserInfo
                {
                    FullName = x.UserInfo.FullName,
                    BirthDate = x.UserInfo.BirthDate,
                    Address = x.UserInfo.Address,
                    PhoneNumber = x.UserInfo.PhoneNumber,
                    Gender = x.UserInfo.Gender
                }
            });
            User.AddRange(users);

            var authorJson = File.ReadAllText("./DbData\\" + typeof(Author).Name + ".json");
            var authorsFromJson = JsonConvert.DeserializeObject<IEnumerable<Author>>(authorJson);
            var authors = authorsFromJson.OrderBy(x => x.Id).Select(x => new Author
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                Country = x.Country,
                ImageUrl = x.ImageUrl
            });
            Author.AddRange(authors);

            var categoryJson = File.ReadAllText("./DbData\\" + typeof(Category).Name + ".json");
            var categoriesFromJson = JsonConvert.DeserializeObject<IEnumerable<Category>>(categoryJson);
            var categories = categoriesFromJson.OrderBy(x => x.Id).Select(x => new Category
            {
                Name = x.Name,
                ImageUrl = x.ImageUrl
            });
            Category.AddRange(categories);

            var bookCategoryJson = File.ReadAllText("./DbData\\" + typeof(BookCategories).Name + ".json");
            var bookCategoriesFromJson = JsonConvert.DeserializeObject<IEnumerable<BookCategories>>(bookCategoryJson);
            var bookCategories = bookCategoriesFromJson.OrderBy(x => x.Id).Select(x => new BookCategories
            {
                Id = x.Id,
                Categories = x.Categories
            });

            coolBookContext.SaveChanges();

            var bookJson = File.ReadAllText("./DbData\\" + typeof(Book).Name + ".json");
            var booksFromJson = JsonConvert.DeserializeObject<IEnumerable<Book>>(bookJson);
            var books = booksFromJson.OrderBy(x => x.Id).Select(x => new Book
            {
                Name = x.Name,
                Author = this.Author.FirstOrDefault(a => a.Id == x.AuthorId),
                Price = x.Price,
                PublishDate = x.PublishDate,
                ImageUrl = x.ImageUrl,
                Views = x.Views,
                Rate = x.Rate,
                Categories = bookCategories.FirstOrDefault(b => b.Id == x.Id).Categories.Select(c => this.Category.FirstOrDefault(cat => cat.Id == c)).ToList<Category>()
            });
            Book.AddRange(books);

            coolBookContext.SaveChanges();

            var reviewJson = File.ReadAllText("./DbData\\" + typeof(Review).Name + ".json");
            var reviewsFromJson = JsonConvert.DeserializeObject<IEnumerable<Review>>(reviewJson);
            var reviews = reviewsFromJson.Select(x => new Review
            {
                Title = x.Title,
                Body = x.Body,
                Rate = x.Rate,
                Date = x.Date,
                User = this.User.FirstOrDefault(u => u.Id == x.UserId),
                Book = this.Book.FirstOrDefault(b => b.Id == x.BookId)
            });
            Review.AddRange(reviews);

            coolBookContext.SaveChanges();
        }

        public DbSet<CoolBook.Models.Book> Book { get; set; }

        public DbSet<CoolBook.Models.Author> Author { get; set; }

        public DbSet<CoolBook.Models.Category> Category { get; set; }

        public DbSet<CoolBook.Models.Review> Review { get; set; }

        public DbSet<CoolBook.Models.User> User { get; set; }

        public DbSet<CoolBook.Models.UserInfo> UserInfo { get; set; }

        public DbSet<CoolBook.Models.Store> Store { get; set; }
    }
}
