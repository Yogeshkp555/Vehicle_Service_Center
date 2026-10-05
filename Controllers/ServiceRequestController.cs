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
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceRequestController(
            IServiceRequestRepository serviceRequestRepository,
            IVehicleRepository vehicleRepository,
            UserManager<ApplicationUser> userManager)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _vehicleRepository = vehicleRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var serviceRequests =
                await _serviceRequestRepository.GetAllAsync();

            return View(serviceRequests);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateServiceRequestViewModel();

            await LoadDropdowns(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateServiceRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model);
                return View(model);
            }

            var customer =
                await _userManager.FindByIdAsync(model.CustomerId);

            if (customer == null ||
                !await _userManager.IsInRoleAsync(customer, "Customer"))
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "Please select a valid customer.");

                await LoadDropdowns(model);
                return View(model);
            }

            var mechanic =
                await _userManager.FindByIdAsync(model.MechanicId);

            if (mechanic == null ||
                !await _userManager.IsInRoleAsync(mechanic, "Mechanic"))
            {
                ModelState.AddModelError(
                    "MechanicId",
                    "Please select a valid mechanic.");

                await LoadDropdowns(model);
                return View(model);
            }

            var vehicle =
                await _vehicleRepository.GetByIdAsync(model.VehicleId);

            if (vehicle == null)
            {
                ModelState.AddModelError(
                    "VehicleId",
                    "Please select a valid vehicle.");

                await LoadDropdowns(model);
                return View(model);
            }

            var serviceRequest = new ServiceRequest
            {
                CustomerId = model.CustomerId,
                VehicleId = model.VehicleId,
                ServiceDescription = model.ServiceDescription,
                SparePartUsedDescription =
                    model.SparePartUsedDescription,
                MechanicId = model.MechanicId,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _serviceRequestRepository.AddAsync(serviceRequest);

            TempData["Success"] =
                "Service request created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var serviceRequest =
                await _serviceRequestRepository.GetByIdAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            var model = new CreateServiceRequestViewModel
            {
                CustomerId = serviceRequest.CustomerId,
                VehicleId = serviceRequest.VehicleId,
                ServiceDescription =
                    serviceRequest.ServiceDescription,
                SparePartUsedDescription =
                    serviceRequest.SparePartUsedDescription,
                MechanicId =
                    serviceRequest.MechanicId ?? string.Empty
            };

            await LoadDropdowns(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CreateServiceRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model);
                return View(model);
            }

            var serviceRequest =
                await _serviceRequestRepository.GetByIdAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            var customer =
                await _userManager.FindByIdAsync(model.CustomerId);

            if (customer == null ||
                !await _userManager.IsInRoleAsync(customer, "Customer"))
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "Please select a valid customer.");

                await LoadDropdowns(model);
                return View(model);
            }

            var mechanic =
                await _userManager.FindByIdAsync(model.MechanicId);

            if (mechanic == null ||
                !await _userManager.IsInRoleAsync(mechanic, "Mechanic"))
            {
                ModelState.AddModelError(
                    "MechanicId",
                    "Please select a valid mechanic.");

                await LoadDropdowns(model);
                return View(model);
            }

            var vehicle =
                await _vehicleRepository.GetByIdAsync(model.VehicleId);

            if (vehicle == null)
            {
                ModelState.AddModelError(
                    "VehicleId",
                    "Please select a valid vehicle.");

                await LoadDropdowns(model);
                return View(model);
            }

            serviceRequest.CustomerId = model.CustomerId;
            serviceRequest.VehicleId = model.VehicleId;
            serviceRequest.ServiceDescription =
                model.ServiceDescription;
            serviceRequest.SparePartUsedDescription =
                model.SparePartUsedDescription;
            serviceRequest.MechanicId = model.MechanicId;

            await _serviceRequestRepository.UpdateAsync(serviceRequest);

            TempData["Success"] =
                "Service request updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceRequest =
                await _serviceRequestRepository.GetByIdAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            await _serviceRequestRepository.DeleteAsync(serviceRequest);

            TempData["Success"] =
                "Service request deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns(
            CreateServiceRequestViewModel model)
        {
            // Customers
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

            // Mechanics
            var mechanics =
                await _userManager.GetUsersInRoleAsync("Mechanic");

            model.Mechanics = mechanics
                .OrderBy(m => m.FullName)
                .Select(m => new SelectListItem
                {
                    Value = m.Id,
                    Text = $"{m.FullName} - {m.Email}"
                })
                .ToList();

            // Vehicles
            var vehicles =
                await _vehicleRepository.GetAllAsync();

            model.Vehicles = vehicles
                .OrderBy(v => v.VehicleNumberPlate)
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.VehicleNumberPlate} - {v.VehicleBrand} {v.VehicleModel}"
                })
                .ToList();
        }
    }
}