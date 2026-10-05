using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Service_Center.Models;
using Vehicle_Service_Center.Repositories;

namespace Vehicle_Service_Center.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IBillRepository _billRepository;

        public CustomerDashboardController(
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyVehicles()
        {
            var customer =
                await _userManager.GetUserAsync(User);

            if (customer == null)
            {
                return Unauthorized();
            }

            var vehicles =
                await _vehicleRepository
                    .GetByCustomerIdAsync(customer.Id);

            return View(vehicles);
        }

        public async Task<IActionResult> MyServices()
        {
            var customer =
                await _userManager.GetUserAsync(User);

            if (customer == null)
            {
                return Unauthorized();
            }

            var services =
                await _serviceRequestRepository
                    .GetByCustomerIdAsync(customer.Id);

            return View(services);
        }

        public async Task<IActionResult> MyBills()
        {
            var customer =
                await _userManager.GetUserAsync(User);

            if (customer == null)
            {
                return Unauthorized();
            }

            var bills =
                await _billRepository
                    .GetByCustomerIdAsync(customer.Id);

            return View(bills);
        }
    }
}