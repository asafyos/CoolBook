using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolBook.Data;
using CoolBook.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoolBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly CoolBookContext _context;

        public BooksController(CoolBookContext context)
        {
            _context = context;
        }

        // GET: Books
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index()
        {
            var coolBookContext = _context.Book.Include(b => b.Author)
                .Include(a => a.Categories)
                .Include(b => b.Reviews);
            return View(await coolBookContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(a => a.Categories)
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.Reviews.Any())
            {
                book.Rate = book.Reviews.Average(r => r.Rate);
            }

            // Increment the views counter of the book
            book.Views++;
            _context.Update(book);
            await _context.SaveChangesAsync();

            return View(book);
        }

        public IActionResult Search()
        {
            ViewData["authors"] = new SelectList(_context.Author, "Id", "Name");
            ViewData["categories"] = new SelectList(_context.Category, "Id", "Name");

            ViewData["books"] = _context.Book.Include(b => b.Author)
                .Include(a => a.Categories)
                .Include(b => b.Reviews);

            return View();
        }

        public IActionResult Find([FromQuery] string Categories, [FromQuery] string AuthorId, [FromQuery] string Name)
        {
            var catList = Categories == null ? new List<int>() : Categories.Split(',').Select(x => int.Parse(x)).ToList();
            var authList = AuthorId == null ? new List<int>() : AuthorId.Split(',').Select(x => int.Parse(x)).ToList();

            var results = _context.Book
                .Include(a => a.Categories)
                .Include(b => b.Author)
                .AsEnumerable()
                .Where(b => ((catList.Count == 0 || catList.All(c => b.Categories.Any(cat => cat.Id == c)))
                          && (authList.Count == 0 || authList.IndexOf(b.AuthorId) != -1)
                          && (string.IsNullOrEmpty(Name) || b.Name.Contains(Name, StringComparison.OrdinalIgnoreCase))))
                .ToList();

            // Prevent JSON conversion recursion
            results.ForEach(b =>
            {
                b.Categories.ForEach(c => c.Books = null);
                b.Author.Books = null;
            });

            return Json(results);
        }

        // GET: Books/Create
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Create()
        {
            ViewData["authors"] = new SelectList(_context.Author, "Id", "Name");
            ViewData["categories"] = new SelectList(_context.Category, "Id", "Name");

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,AuthorId,Price,PublishDate,ImageUrl")] Book book, List<int>? categories)
        {
            if (ModelState.IsValid)
            {
                // Add categories to book
                AddCategories(book, categories);

                book.Views = 0;
                book.Rate = 0.0;

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.Include(b => b.Categories)
                                          .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", book.AuthorId);
            ViewData["Categories"] = new SelectList(_context.Category, "Id", "Name", book.Categories);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AuthorId,Price,PublishDate,ImageUrl")] Book book, List<int>? categories)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    UpdateBook(book, categories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        // Adds the actual category instances to the book
        public void AddCategories(Book book, List<int>? categories)
        {
            if (book.Categories == null)
                book.Categories = new List<Category>();

            // Make the matching between categories and books, delete previous
            foreach (var CategoryId in categories)
            {
                book.Categories.Add(_context.Category.Find(CategoryId));
            }
        }

        // This is used to update safetly the many many connection with categories
        // Update the original book instance with the new information
        public void UpdateBook(Book newBook, List<int>? categories)
        {
            var originalBook = _context.Book.Include("Categories").FirstOrDefault(b => b.Id == newBook.Id);
            originalBook.Name = newBook.Name;
            originalBook.AuthorId = newBook.AuthorId;
            originalBook.ImageUrl = newBook.ImageUrl;
            originalBook.Price = newBook.Price;
            originalBook.PublishDate = newBook.PublishDate;
            originalBook.Categories.Clear();
            foreach (var catId in categories)
            {
                originalBook.Categories.Add(_context.Category.Find(catId));
            }

            _context.Update(originalBook);
        }
    }
}
