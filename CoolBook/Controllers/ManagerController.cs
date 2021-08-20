using CoolBook.Data;
using CoolBook.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Graphs()
        {
            ViewData["WatchedBooks"] = GetWatchedBooks(3);
            ViewData["BestBooks"] = GetBestBooks(3);

            var data = new[] {
                new { label = "Abulia", count = 10 },
                new { label = "Betelgeuse", count = 20 },
                new { label = "Cantaloupe", count = 30 },
                new { label = "Dijkstra", count = 40 }
            };


            //ViewData["CatAmounts"] = GetCatAmounts();

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
            public string CategoryName { get; set; }
            public int BookAmount { get; set; }
        }

        public JsonResult GetCatAmounts()
        {
            var result = new List<Tuple<string, int>>(_context.Category.Count());
            _context.Category.Include(c => c.Books)
                            .ToList()
                            .ForEach(c => result.Add(new Tuple<string, int>(c.Name, c.Books.Count)));

            result.Sort((ca1, ca2) => ca2.Item2.CompareTo(ca1.Item2));

            return new JsonResult(result);
        }
    }
}
