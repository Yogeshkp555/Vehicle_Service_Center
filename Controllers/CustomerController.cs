using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Service_Center.Models;
using Vehicle_Service_Center.ViewModels;

namespace Vehicle_Service_Center.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Customer
        public async Task<IActionResult> Index()
        {
            var customers =
                await _userManager.GetUsersInRoleAsync("Customer");

            return View(customers);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateCustomerViewModel model)
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

            var customer = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(
                customer,
                model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(
                    customer,
                    "Customer");

                TempData["Success"] =
                    "Customer created successfully.";

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

        // GET: /Customer/Edit/id
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var customer =
                await _userManager.FindByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var roles =
                await _userManager.GetRolesAsync(customer);

            if (!roles.Contains("Customer"))
            {
                return NotFound();
            }

            var model = new CreateCustomerViewModel
            {
                FullName = customer.FullName,
                Email = customer.Email ?? string.Empty,
                PhoneNumber = customer.PhoneNumber ?? string.Empty,
                Address = customer.Address ?? string.Empty,

                // Password is not required during edit.
                Password = "Temp@123"
            };

            ViewBag.CustomerId = customer.Id;

            return View(model);
        }

        // POST: /Customer/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id,
            CreateCustomerViewModel model)
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
                ViewBag.CustomerId = id;
                return View(model);
            }

            var customer =
                await _userManager.FindByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var roles =
                await _userManager.GetRolesAsync(customer);

            if (!roles.Contains("Customer"))
            {
                return NotFound();
            }

            customer.FullName = model.FullName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.Address = model.Address;

            var result =
                await _userManager.UpdateAsync(customer);

            if (result.Succeeded)
            {
                TempData["Success"] =
                    "Customer updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            ViewBag.CustomerId = id;

            return View(model);
        }

        // POST: /Customer/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var customer =
                await _userManager.FindByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var roles =
                await _userManager.GetRolesAsync(customer);

            if (!roles.Contains("Customer"))
            {
                return NotFound();
            }

            var result =
                await _userManager.DeleteAsync(customer);

            if (result.Succeeded)
            {
                TempData["Success"] =
                    "Customer deleted successfully.";
            }
            else
            {
                TempData["Error"] =
                    "Unable to delete customer.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}