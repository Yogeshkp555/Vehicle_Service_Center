using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vehicle_Service_Center.ViewModels
{
    public class CreateBillViewModel
    {
        [Required]
        [Display(Name = "Service Request")]
        public int ServiceRequestId { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Spare Part Charge")]
        public decimal SparePartCharge { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Labour Charge")]
        public decimal LabourCharge { get; set; }

        [Required]
        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; } = "Pending";

        public List<SelectListItem> ServiceRequests { get; set; }
            = new List<SelectListItem>();
    }
}