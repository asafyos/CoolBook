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

            return View();
        }

        public List<Book> GetWatchedBooks(int amount)
        {
            return _context.Book.OrderByDescending(b => b.Views).Take(amount).ToList();
        }
    }
}
