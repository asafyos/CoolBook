using CoolBook.Data;
using CoolBook.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoolBook.Controllers
{
    public class UsersController : Controller
    {
        private readonly CoolBookContext _context;

        public UsersController(CoolBookContext context)
        {
            _context = context;
        }

        public class FullUser
        {
            public User User { get; set; }
            public UserInfo UserInfo { get; set; }
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = _context.User.ToList();
            users.ForEach(u => u.Password = "");
            return View(users);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _context.User
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(m => m.UserName == HttpContext.User.Identity.Name);

            return View(new FullUser
            {
                User = user,
                UserInfo = user.UserInfo
            });
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Password,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Update/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Validate this is the logged in user
            if (id != int.Parse(HttpContext.User.FindFirst("UserId").Value))
            {
                return AccessDenied();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        
        // POST: Users/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string Email, string PasswordConfirmation, string Password)
        {
            // Validate this is the logged in user
            if (id != int.Parse(HttpContext.User.FindFirst("UserId").Value))
            {
                return AccessDenied();
            }

            var user = _context.User.Find(id);

            // Validate the password entered
            if (user.Password != PasswordConfirmation)
            {
                return RedirectToAction(nameof(WrongPassword));
            }

            // Update fields
            user.Email = Email;
            user.Password = Password;
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return RedirectToAction(nameof(Profile));
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,Password,Email")] User user)
        {
            if (!ModelState.IsValid)
            {
                //TODO: handle error
                return null;
            }

            var result = await _context.User.FirstOrDefaultAsync(u => (u.UserName == user.UserName || u.Email == user.Email));

            if (result != null)
            {
                //TODO: handle error
                return null;
            }
            
            // Defualt values
            user.Role = UserRole.Client;
            user.UserInfo = new UserInfo { FullName = user.UserName, BirthDate = DateTime.Now };

            _context.Add(user);
            await _context.SaveChangesAsync();

            await Signin(user);

            return RedirectToAction(nameof(Index), "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(String UserName, String Password, [FromQuery] string redirect)
        {
            var result = from u in _context.User
                         where u.UserName == UserName
                            && u.Password == Password
                         select u;

            if (!result.Any())
            {
                return RedirectToAction(nameof(WrongLogin));
            }

            await Signin(result.First());

            if (redirect != null)
            {
                return Redirect(redirect);
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Logout([FromQuery] string redirect)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (redirect != null)
            {
                return Redirect(redirect);
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private async Task Signin(User account)
        {
            var claims = new List<Claim> {
                new Claim("UserId", account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        // Bad login page
        public ActionResult WrongLogin()
        {
            return View();
        }

        // Wrong password page
        public ActionResult WrongPassword()
        {
            return View();
        }

        // Access Denied page
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
