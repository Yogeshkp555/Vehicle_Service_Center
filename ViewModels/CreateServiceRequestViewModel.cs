using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vehicle_Service_Center.ViewModels
{
    public class CreateServiceRequestViewModel
    {
        [Required]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Service Description")]
        public string ServiceDescription { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Spare Part Used Description")]
        public string? SparePartUsedDescription { get; set; }

        [Required]
        [Display(Name = "Mechanic")]
        public string MechanicId { get; set; } = string.Empty;

        public List<SelectListItem> Customers { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> Vehicles { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> Mechanics { get; set; }
            = new List<SelectListItem>();
    }
}