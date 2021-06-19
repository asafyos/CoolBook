using Microsoft.EntityFrameworkCore;

namespace CoolBook.Data
{
    public class CoolBookContext : DbContext
    {
        public CoolBookContext(DbContextOptions<CoolBookContext> options)
            : base(options)
        {
        }

        public DbSet<CoolBook.Models.Book> Book { get; set; }

        public DbSet<CoolBook.Models.Author> Author { get; set; }

        public DbSet<CoolBook.Models.Category> Category { get; set; }

        public DbSet<CoolBook.Models.Review> Review { get; set; }

        public DbSet<CoolBook.Models.User> User { get; set; }

        public DbSet<CoolBook.Models.UserInfo> UserInfo { get; set; }
    }
}
