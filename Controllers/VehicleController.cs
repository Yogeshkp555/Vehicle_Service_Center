using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle_Service_Center.Models;
using Vehicle_Service_Center.Repositories;
using Vehicle_Service_Center.ViewModels;

namespace Vehicle_Service_Center.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehicleController(
            IVehicleRepository vehicleRepository,
            UserManager<ApplicationUser> userManager)
        {
            _vehicleRepository = vehicleRepository;
            _userManager = userManager;
        }

        // GET: /Vehicle
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            return View(vehicles);
        }

        // GET: /Vehicle/Create
        public async Task<IActionResult> Create()
        {
            var model = new CreateVehicleViewModel();

            await LoadCustomers(model);

            return View(model);
        }

        // POST: /Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateVehicleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomers(model);
                return View(model);
            }

            var customer = await _userManager.FindByIdAsync(
                model.CustomerId);

            if (customer == null ||
                !await _userManager.IsInRoleAsync(customer, "Customer"))
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "Please select a valid customer.");

                await LoadCustomers(model);
                return View(model);
            }

            var vehicle = new Vehicle
            {
                VehicleNumberPlate = model.VehicleNumberPlate,
                CustomerId = model.CustomerId,
                VehicleType = model.VehicleType,
                VehicleBrand = model.VehicleBrand,
                VehicleModel = model.VehicleModel,
                Issue = model.Issue
            };

            await _vehicleRepository.AddAsync(vehicle);

            TempData["Success"] =
                "Vehicle added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Vehicle/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle =
                await _vehicleRepository.GetByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var model = new CreateVehicleViewModel
            {
                VehicleNumberPlate = vehicle.VehicleNumberPlate,
                CustomerId = vehicle.CustomerId,
                VehicleType = vehicle.VehicleType,
                VehicleBrand = vehicle.VehicleBrand,
                VehicleModel = vehicle.VehicleModel,
                Issue = vehicle.Issue
            };

            await LoadCustomers(model);

            return View(model);
        }

        // POST: /Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CreateVehicleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomers(model);
                return View(model);
            }

            var vehicle =
                await _vehicleRepository.GetByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var customer = await _userManager.FindByIdAsync(
                model.CustomerId);

            if (customer == null ||
                !await _userManager.IsInRoleAsync(customer, "Customer"))
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "Please select a valid customer.");

                await LoadCustomers(model);
                return View(model);
            }

            vehicle.VehicleNumberPlate = model.VehicleNumberPlate;
            vehicle.CustomerId = model.CustomerId;
            vehicle.VehicleType = model.VehicleType;
            vehicle.VehicleBrand = model.VehicleBrand;
            vehicle.VehicleModel = model.VehicleModel;
            vehicle.Issue = model.Issue;

            await _vehicleRepository.UpdateAsync(vehicle);

            TempData["Success"] =
                "Vehicle updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Vehicle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle =
                await _vehicleRepository.GetByIdAsync(id);

            if (vehicle == null)
            {
                TempData["Error"] = "Vehicle not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _vehicleRepository.DeleteAsync(vehicle);

                TempData["Success"] =
                    "Vehicle deleted successfully.";
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                TempData["Error"] =
                    "This vehicle cannot be deleted because it is linked to one or more service requests. Please manage those service requests first.";
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task LoadCustomers(
            CreateVehicleViewModel model)
        {
            var customers =
                await _userManager.GetUsersInRoleAsync("Customer");

            model.Customers = customers
                .OrderBy(c => c.FullName)
                .Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = $"{c.FullName} - {c.Email}"
                })
                .ToList();
        }
    }
}