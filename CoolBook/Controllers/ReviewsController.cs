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
    public class ReviewsController : Controller
    {
        private readonly CoolBookContext _context;

        public ReviewsController(CoolBookContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var coolBookContext = _context.Review.Include(r => r.Book).Include(r => r.User);
            return View(await coolBookContext.ToListAsync());
        }

        public IActionResult Search([FromQuery] string book, [FromQuery] int? rate, [FromQuery] string review)
        {
            if (book == null) book = "";
            if (review == null) review = "";
            if (rate == null) rate = 0;

            return View(_context.Review.Include(r => r.Book).Include(r => r.User).AsEnumerable()
                .Where(r => (r.Body.Contains(review, StringComparison.InvariantCultureIgnoreCase)
                          || r.Title.Contains(review, StringComparison.InvariantCultureIgnoreCase))
                         && r.Book.Name.Contains(book, StringComparison.InvariantCultureIgnoreCase)
                         && (rate == 0 || r.Rate == rate)));
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Title,Body,Rate,BookId")] Review review)
        {
            // Validate a user is logged in
            if (HttpContext.User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // insert from session
            review.UserId = int.Parse(HttpContext.User.FindFirst("UserId").Value);

            // Update the date
            review.Date = DateTime.Now;

            // Update the rate
            var book = await _context.Book.Include(b => b.Reviews).FirstOrDefaultAsync(b => b.Id == review.BookId);
            book.Rate = Math.Round(book.Reviews.Average(r => r.Rate), 2);

            _context.Update(book);

            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Books", new { id = review.BookId });
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            // Validate a user is logged in
            if (HttpContext.User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            
            // Admins can edit all reviews, others can only edit their own
            if ((!HttpContext.User.IsInRole("Admin")) &&
                (review.UserId != int.Parse(HttpContext.User.FindFirst("UserId").Value)))
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Name", review.BookId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "UserName", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Rate,UserId,BookId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            // Validate a user is logged in
            if (HttpContext.User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // Admins can edit all reviews, others can only edit their own
            if ((!HttpContext.User.IsInRole("Admin")) &&
                (review.UserId != int.Parse(HttpContext.User.FindFirst("UserId").Value)))
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Name", review.BookId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "UserName", review.UserId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}
