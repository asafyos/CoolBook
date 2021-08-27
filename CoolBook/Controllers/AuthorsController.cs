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
using System.ComponentModel.DataAnnotations;

namespace CoolBook.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly CoolBookContext _context;

        public AuthorsController(CoolBookContext context)
        {
            _context = context;
        }

        public class FullAuthor
        {
            public int Id { get; set; }
            public string Name { get; set; }

            [Required, Display(Name = "Birth Date"), DataType(DataType.Date)]
            public DateTime BirthDate { get; set; }
            public Gender Gender { get; set; }
            public string Country { get; set; }

            [Display(Name = "Books Count")]
            public int BookCount { get; set; }
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Author.Join(
                _context.Book,
                author => author.Id,
                book => book.AuthorId,
                (author, book) => new
                {
                    Id = author.Id,
                    Name = author.Name,
                    BirthDate = author.BirthDate,
                    Gender = author.Gender,
                    Country = author.Country,
                    BookId = book.Id
                })
                .GroupBy(x => new
                {
                    x.Id,
                    x.Name,
                    x.BirthDate,
                    x.Gender,
                    x.Country
                })
                .Select(x => new FullAuthor
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    BirthDate = x.Key.BirthDate,
                    Gender = x.Key.Gender,
                    Country = x.Key.Country,
                    BookCount = x.Select(a => a.BookId).Distinct().Count()
                }).ToListAsync());

            return View(await _context.Author.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .Include(a => a.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,Gender,Country")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthDate,Gender,Country")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Author.FindAsync(id);
            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.Id == id);
        }
    }
}
