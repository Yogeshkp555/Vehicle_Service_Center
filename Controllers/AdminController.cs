using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Service_Center.Models;
using Vehicle_Service_Center.Repositories;

namespace Vehicle_Service_Center.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IBillRepository _billRepository;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            IVehicleRepository vehicleRepository,
            IServiceRequestRepository serviceRequestRepository,
            IBillRepository billRepository)
        {
            _userManager = userManager;
            _vehicleRepository = vehicleRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _billRepository = billRepository;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Total Customers
            var customers =
                await _userManager.GetUsersInRoleAsync("Customer");

            // Total Mechanics
            var mechanics =
                await _userManager.GetUsersInRoleAsync("Mechanic");

            // Total Vehicles
            var vehicles =
                await _vehicleRepository.GetAllAsync();

            // All Service Requests
            var services =
                await _serviceRequestRepository.GetAllAsync();

            // Pending Services
            var pendingServices =
                services.Count(s => s.Status == "Pending");

            // Completed Services
            var completedServices =
                services.Count(s => s.Status == "Completed");

            // Total Bills
            var bills =
                await _billRepository.GetAllAsync();

            ViewBag.TotalCustomers = customers.Count;
            ViewBag.TotalMechanics = mechanics.Count;
            ViewBag.TotalVehicles = vehicles.Count;
            ViewBag.PendingServices = pendingServices;
            ViewBag.CompletedServices = completedServices;
            ViewBag.TotalBills = bills.Count;

            return View();
        }
    }
}