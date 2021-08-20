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
            return View();
        }

        public JsonResult GetWatchedBooks()
        {
            var AMOUNT_OF_MOST_WATCHED = 8;
            return new JsonResult(_context.Book.OrderByDescending(b => b.Views)
                                               .Take(AMOUNT_OF_MOST_WATCHED)
                                               .Select(b => new { b.Name, b.Views })
                                               .ToList());
        }

        public JsonResult GetBestBooks()
        {
            var AMOUNT_OF_BEST_BOOKS = 8;
            return new JsonResult(_context.Book.OrderByDescending(b => b.Rate)
                                               .Take(AMOUNT_OF_BEST_BOOKS)
                                               .Select(b => new { b.Name, b.Rate })
                                               .ToList());
        }

        public JsonResult GetCatAmounts()
        {
            return new JsonResult(_context.Category.Include(c => c.Books)
                                                   .Select(c => new { c.Name, c.Books.Count })
                                                   .ToList()
                                                   .OrderByDescending(c => c.Count));                            

        }
    }
}
