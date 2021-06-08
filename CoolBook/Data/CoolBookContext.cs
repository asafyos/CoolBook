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
        public CoolBookContext (DbContextOptions<CoolBookContext> options)
            : base(options)
        {
        }

        public DbSet<CoolBook.Models.Book> Book { get; set; }
    }
}
