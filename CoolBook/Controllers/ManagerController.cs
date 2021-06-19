using CoolBook.Data;
using CoolBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Controllers
{
    public class ManagerController : Controller
    {
        private readonly CoolBookContext _context;

        public ManagerController(CoolBookContext context)
        {
            _context = context;
        }

        // GET: ManagerController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Graphs()
        {
            ViewData["WatchedBooks"] = GetWatchedBooks(3);
            ViewData["BestBooks"] = GetBestBooks(3);
            ViewData["CatAmounts"] = GetCatAmounts();

            return View();
        }

        public List<Book> GetWatchedBooks(int amount)
        {
            return _context.Book.OrderByDescending(b => b.Views).Take(amount).ToList();
        }

        public List<Book> GetBestBooks(int amount)
        {
            return _context.Book.OrderByDescending(b => b.Rate).Take(amount).ToList();
        }

        public class CategoryAmount
        {
            public Category Category { get; set; }
            public int BookAmount { get; set; }
        }

        public List<CategoryAmount> GetCatAmounts()
        {
            List<CategoryAmount> result = new List<CategoryAmount>(_context.Category.Count());
            _context.Category.Include(c => c.Books)
                            .ToList() 
                            .ForEach(c => result.Add(new CategoryAmount { 
                                                            Category = c, 
                                                            BookAmount = c.Books.Count() }));

            result.Sort((ca1, ca2) => ca2.BookAmount.CompareTo(ca1.BookAmount));

            return result;
        }
    }
}
