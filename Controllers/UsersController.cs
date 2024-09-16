using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarManagement.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Controllers
{
    [Authorize(Roles = "WebAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    return View(user);
                }

                // Check password constraints
                if (!IsValidPassword(password))
                {
                    ModelState.AddModelError(string.Empty, "Password must contain at least one lowercase letter, one uppercase letter, one symbol, and one number.");
                    return View(user);
                }

                user.UserName = user.Email;
                user.EmailConfirmed = true; // Set email as confirmed

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }


        // Helper method to validate password
        private bool IsValidPassword(string password)
        {
            bool hasUpperCase = password.Any(c => char.IsUpper(c));
            bool hasLowerCase = password.Any(c => char.IsLower(c));
            bool hasSymbol = password.Any(c => char.IsSymbol(c) || char.IsPunctuation(c));
            bool hasDigit = password.Any(c => char.IsDigit(c));

            return hasUpperCase && hasLowerCase && hasSymbol && hasDigit;
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.UserName = user.UserName;

                    var result = await _userManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }


        private async Task<bool> UserExists(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user != null;
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is the admin and prevent deletion
            if (user.UserName.Equals("admin@carmanagement.com", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "The admin user cannot be deleted.");
                return RedirectToAction(nameof(Delete), new { id });
            }

            // Proceed with deletion if not the admin
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

    }
}
