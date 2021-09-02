using Microsoft.EntityFrameworkCore;
using System;

namespace CoolBook.Data
{
    public class CoolBookContext : DbContext
    {
        public CoolBookContext(DbContextOptions<CoolBookContext> options)
            : base(options)
        {
            if (Database.EnsureCreated()) InitData(this);
        }

        private void InitData(CoolBookContext coolBookContext)
        {
            var adminInfo = new CoolBook.Models.UserInfo
            {
                PhoneNumber = "050-5555555",
                BirthDate = new DateTime(0001, 1, 1,0, 0, 0),
                Address = "here",
                FullName = "Admin",
                Gender = Models.Gender.Female
            };

            var admin = new CoolBook.Models.User
            {
                UserName = "admin",
                Email = "admin@admin",
                Password = "admin",
                Role = 0,
                UserInfo = adminInfo
            };

            adminInfo.User = admin;

            coolBookContext.User.AddRange(admin);
            coolBookContext.UserInfo.AddRange(adminInfo);

            coolBookContext.SaveChanges();
        }

        public DbSet<CoolBook.Models.Book> Book { get; set; }

        public DbSet<CoolBook.Models.Author> Author { get; set; }

        public DbSet<CoolBook.Models.Category> Category { get; set; }

        public DbSet<CoolBook.Models.Review> Review { get; set; }

        public DbSet<CoolBook.Models.User> User { get; set; }

        public DbSet<CoolBook.Models.UserInfo> UserInfo { get; set; }
    }
}
