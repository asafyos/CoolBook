using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolBook.Data;
using CoolBook.Models;

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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["authors"] = new SelectList(_context.Author, "Id", "Name");
            ViewData["categories"] = new SelectList(_context.Category, "Id", "Name");

            return View();
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
                .Where(b => ((catList.Count == 0 || catList.All(c=>b.Categories.Any(cat=>cat.Id==c)))
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

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AuthorId,Price,PublishDate,ImageUrl,Categories")] Book book, List<int>? categories)
        {
            if (ModelState.IsValid)
            {
                // Update the categories that this book is in
                UpdateCategories(book, categories);

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
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
                    UpdateCategories(book, categories);
                    _context.Update(book);
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

        public void UpdateCategories(Book book, List<int>? categories)
        {
            if (categories == null)
                return;

            // Make the matching between categories and books, delete previous
            foreach (var CategoryId in categories)
            {
                var category = _context.Category.Find(CategoryId);
                if (category.Books == null)
                    category.Books = new List<Book>();

                if (book.Categories == null)
                    book.Categories = new List<Category>();

                category.Books.Add(book);
                book.Categories.Add(category);
            }
        }
    }
}
