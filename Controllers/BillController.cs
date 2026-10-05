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
    public class BillController : Controller
    {
        private readonly IBillRepository _billRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillController(
            IBillRepository billRepository,
            IServiceRequestRepository serviceRequestRepository,
            UserManager<ApplicationUser> userManager)
        {
            _billRepository = billRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var bills = await _billRepository.GetAllAsync();

            return View(bills);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateBillViewModel();

            await LoadServiceRequests(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateBillViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadServiceRequests(model);
                return View(model);
            }

            var serviceRequest =
                await _serviceRequestRepository
                    .GetByIdAsync(model.ServiceRequestId);

            if (serviceRequest == null)
            {
                ModelState.AddModelError(
                    "ServiceRequestId",
                    "Please select a valid service request.");

                await LoadServiceRequests(model);
                return View(model);
            }

            var bill = new Bill
            {
                ServiceRequestId = serviceRequest.Id,
                CustomerId = serviceRequest.CustomerId,
                VehicleNumberPlate =
                    serviceRequest.Vehicle?.VehicleNumberPlate ?? "",
                ServiceDescription =
                    serviceRequest.ServiceDescription,
                SparePartCharge = model.SparePartCharge,
                LabourCharge = model.LabourCharge,
                PaymentType = model.PaymentType,
                PaymentStatus = model.PaymentStatus,
                CreatedAt = DateTime.Now
            };

            await _billRepository.AddAsync(bill);

            TempData["Success"] =
                "Bill created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var bill = await _billRepository.GetByIdAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            var model = new CreateBillViewModel
            {
                ServiceRequestId = bill.ServiceRequestId,
                SparePartCharge = bill.SparePartCharge,
                LabourCharge = bill.LabourCharge,
                PaymentType = bill.PaymentType,
                PaymentStatus = bill.PaymentStatus
            };

            await LoadServiceRequests(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CreateBillViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadServiceRequests(model);
                return View(model);
            }

            var bill = await _billRepository.GetByIdAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            var serviceRequest =
                await _serviceRequestRepository
                    .GetByIdAsync(model.ServiceRequestId);

            if (serviceRequest == null)
            {
                ModelState.AddModelError(
                    "ServiceRequestId",
                    "Please select a valid service request.");

                await LoadServiceRequests(model);
                return View(model);
            }

            bill.ServiceRequestId = serviceRequest.Id;
            bill.CustomerId = serviceRequest.CustomerId;
            bill.VehicleNumberPlate =
                serviceRequest.Vehicle?.VehicleNumberPlate ?? "";
            bill.ServiceDescription =
                serviceRequest.ServiceDescription;

            bill.SparePartCharge = model.SparePartCharge;
            bill.LabourCharge = model.LabourCharge;
            bill.PaymentType = model.PaymentType;
            bill.PaymentStatus = model.PaymentStatus;

            await _billRepository.UpdateAsync(bill);

            TempData["Success"] =
                "Bill updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var bill = await _billRepository.GetByIdAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            await _billRepository.DeleteAsync(bill);

            TempData["Success"] =
                "Bill deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadServiceRequests(
            CreateBillViewModel model)
        {
            var serviceRequests =
                await _serviceRequestRepository.GetAllAsync();

            model.ServiceRequests = serviceRequests
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text =
                        $"{s.Customer?.FullName ?? "N/A"} - " +
                        $"{s.Vehicle?.VehicleNumberPlate ?? "N/A"} - " +
                        $"{s.ServiceDescription}"
                })
                .ToList();
        }
    }
}