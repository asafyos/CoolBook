using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoolBook.Models;

namespace CoolBook.Data
{
    public class CoolBookContext : DbContext
    {
        public CoolBookContext(DbContextOptions<CoolBookContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                InitData();
            }
        }

        public DbSet<CoolBook.Models.Book> Book { get; set; }

        public DbSet<CoolBook.Models.Author> Author { get; set; }

        public DbSet<CoolBook.Models.Category> Category { get; set; }

        public DbSet<CoolBook.Models.Review> Review { get; set; }

        public DbSet<CoolBook.Models.User> User { get; set; }

        public DbSet<CoolBook.Models.UserInfo> UserInfo { get; set; }

        private void InitData()
        {
            var cat1 = new Category
            {
                Id = 1,
                Name = "Fantasy"
            };

            var cat2 = new Category
            {
                Id = 2,
                Name = "Young adult literature"
            };

            var cat3 = new Category
            {
                Id = 3,
                Name = "Horror literature"
            };

            var cat4 = new Category
            {
                Id = 4,
                Name = "Dark fantasy"
            };

            var cat5 = new Category
            {
                Id = 5,
                Name = "Fiction"
            };

            var author1 = new Author
            {
                Id = 1,
                Name = "Ransom Riggs",
                Country = "America",
                Gender = Gender.Male,
                BirthDate = new DateTime(1979, 2, 3)
            };

            var author2 = new Author
            {
                Id = 2,
                Name = "Gregg Hurwitz",
                Country = "America",
                Gender = Gender.Male,
                BirthDate = new DateTime(1973, 1, 1)
            };

            var book1 = new Book
            {
                Id = 1,
                Name = "Miss Peregrine's Home for Peculiar Children",
                PublishDate = new DateTime(2011, 6, 7),
                Price = 89.9,
                ImageUrl = "https://en.wikipedia.org/wiki/File:MissPeregrineCover.jpg",
                Categories = new List<Category> { cat1, cat2 },
                Author = author1
            };

            var book2 = new Book
            {
                Id = 2,
                Name = "Hollow City",
                PublishDate = new DateTime(2014, 1, 4),
                Price = 99.9,
                ImageUrl = "https://en.wikipedia.org/wiki/File:Hollow_City_(novel)_cover.jpg",
                Categories = new List<Category> { cat1, cat3 },
                Author = author1
            };

            var book3 = new Book
            {
                Id = 3,
                Name = "Library of Souls",
                PublishDate = new DateTime(2015, 9, 22),
                Price = 99.9,
                ImageUrl = "https://en.wikipedia.org/wiki/File:Library_of_Souls_cover.jpg",
                Categories = new List<Category> { cat1, cat3, cat4 },
                Author = author1
            };

            var book4 = new Book
            {
                Id = 4,
                Name = "Orphan X",
                PublishDate = new DateTime(2016, 1, 19),
                Price = 85.9,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/7/70/Orphan_X_Book_cover.jpg",
                Categories = new List<Category> { cat5 },
                Author = author2
            };

            var user1 = new User
            {
                Id = 1,
                UserName = "Asaf",
                Email = "a@b.c",
                Role = UserRole.Admin,
                Password = "admin"
            };

            var userInfo1 = new UserInfo
            {
                Id = 1,
                Gender = Gender.Male,
                FullName = "Asaf Yosef",
                BirthDate = new DateTime(1998, 3, 3),
                PhoneNumber = "+972 50-233-1298",
                User = user1,
                Address = "Rosh HaAyin"
            };

            this.Author.AddRange(author1, author2);
            this.Category.AddRange(cat1, cat2, cat3, cat4, cat5);
            this.Book.AddRange(book1, book2, book3, book4);
            this.User.Add(user1);
            this.UserInfo.Add(userInfo1);

            this.SaveChanges();

        }
    }
}
