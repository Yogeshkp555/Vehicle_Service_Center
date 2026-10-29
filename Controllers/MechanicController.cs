using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Service_Center.Models;
using Vehicle_Service_Center.ViewModels;

namespace Vehicle_Service_Center.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MechanicController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MechanicController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Mechanic
        public async Task<IActionResult> Index()
        {
            var mechanics =
                await _userManager.GetUsersInRoleAsync("Mechanic");

            return View(mechanics);
        }

        // GET: /Mechanic/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Mechanic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateMechanicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "A user with this email already exists.");

                return View(model);
            }

            var mechanic = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(
                mechanic,
                model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(
                    mechanic,
                    "Mechanic");

                TempData["Success"] =
                    "Mechanic created successfully.";

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            return View(model);
        }

        // GET: /Mechanic/Edit/id
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var mechanic =
                await _userManager.FindByIdAsync(id);

            if (mechanic == null)
            {
                return NotFound();
            }

            var roles =
                await _userManager.GetRolesAsync(mechanic);

            if (!roles.Contains("Mechanic"))
            {
                return NotFound();
            }

            var model = new CreateMechanicViewModel
            {
                FullName = mechanic.FullName,
                Email = mechanic.Email ?? string.Empty,
                PhoneNumber = mechanic.PhoneNumber ?? string.Empty,
                Address = mechanic.Address ?? string.Empty,

                // Password is not required during edit.
                Password = "Temp@123"
            };

            ViewBag.MechanicId = mechanic.Id;

            return View(model);
        }

        // POST: /Mechanic/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id,
            CreateMechanicViewModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Password is not being changed during edit.
            // Remove password validation from ModelState.
            ModelState.Remove(nameof(model.Password));

            if (!ModelState.IsValid)
            {
                ViewBag.MechanicId = id;
                return View(model);
            }

            var mechanic =
                await _userManager.FindByIdAsync(id);

            if (mechanic == null)
            {
                return NotFound();
            }

            var roles =
                await _userManager.GetRolesAsync(mechanic);

            if (!roles.Contains("Mechanic"))
            {
                return NotFound();
            }

            mechanic.FullName = model.FullName;
            mechanic.PhoneNumber = model.PhoneNumber;
            mechanic.Address = model.Address;

            var result =
                await _userManager.UpdateAsync(mechanic);

            if (result.Succeeded)
            {
                TempData["Success"] =
                    "Mechanic updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            ViewBag.MechanicId = id;

            return View(model);
        }

        // POST: /Mechanic/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var mechanic = await _userManager.FindByIdAsync(id);

            if (mechanic == null)
            {
                TempData["Error"] = "Mechanic not found.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(mechanic);

            if (!roles.Contains("Mechanic"))
            {
                TempData["Error"] = "This user is not a mechanic.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var result = await _userManager.DeleteAsync(mechanic);

                if (result.Succeeded)
                {
                    TempData["Success"] =
                        "Mechanic deleted successfully.";
                }
                else
                {
                    TempData["Error"] =
                        "Unable to delete mechanic.";
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                TempData["Error"] =
                    "This mechanic cannot be deleted because the mechanic is assigned to one or more service requests. Please reassign or complete those service requests first.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}