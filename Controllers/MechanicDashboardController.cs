using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Service_Center.Data;
using Vehicle_Service_Center.Models;
using Vehicle_Service_Center.Repositories;
using Vehicle_Service_Center.ViewModels;

namespace Vehicle_Service_Center.Controllers
{
    [Authorize(Roles = "Mechanic")]
    public class MechanicDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        public MechanicDashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IServiceRequestRepository serviceRequestRepository)
        {
            _context = context;
            _userManager = userManager;
            _serviceRequestRepository = serviceRequestRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyServices()
        {
            var mechanic =
                await _userManager.GetUserAsync(User);

            if (mechanic == null)
            {
                return Unauthorized();
            }

            var services =
                await _serviceRequestRepository
                    .GetByMechanicIdAsync(mechanic.Id);

            return View(services);
        }

        public async Task<IActionResult> EditService(int id)
        {
            var mechanic =
                await _userManager.GetUserAsync(User);

            if (mechanic == null)
            {
                return Unauthorized();
            }

            var serviceRequest =
                await _serviceRequestRepository.GetByIdAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            // Mechanic can edit only their own assigned service
            if (serviceRequest.MechanicId != mechanic.Id)
            {
                return Forbid();
            }

            var model = new UpdateMechanicServiceViewModel
            {
                Id = serviceRequest.Id,
                ServiceDescription =
                    serviceRequest.ServiceDescription,
                SparePartUsedDescription =
                    serviceRequest.SparePartUsedDescription,
                Status = serviceRequest.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(
            int id,
            UpdateMechanicServiceViewModel model)
        {
            var mechanic =
                await _userManager.GetUserAsync(User);

            if (mechanic == null)
            {
                return Unauthorized();
            }

            var serviceRequest =
                await _serviceRequestRepository.GetByIdAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            // Mechanic can update only their own assigned service
            if (serviceRequest.MechanicId != mechanic.Id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Allow only valid statuses
            var allowedStatuses = new[]
            {
                "Pending",
                "InProgress",
                "Completed"
            };

            if (!allowedStatuses.Contains(model.Status))
            {
                ModelState.AddModelError(
                    "Status",
                    "Invalid service status.");

                return View(model);
            }

            serviceRequest.ServiceDescription =
                model.ServiceDescription;

            serviceRequest.SparePartUsedDescription =
                model.SparePartUsedDescription;

            serviceRequest.Status =
                model.Status;

            await _serviceRequestRepository
                .UpdateAsync(serviceRequest);

            TempData["Success"] =
                "Service updated successfully.";

            return RedirectToAction(nameof(MyServices));
        }
    }
}