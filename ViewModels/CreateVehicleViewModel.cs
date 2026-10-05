using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vehicle_Service_Center.ViewModels
{
    public class CreateVehicleViewModel
    {
        [Required]
        [Display(Name = "Vehicle Number Plate")]
        public string VehicleNumberPlate { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Vehicle Brand")]
        public string VehicleBrand { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Vehicle Model")]
        public string VehicleModel { get; set; } = string.Empty;

        [Display(Name = "Issue")]
        public string? Issue { get; set; }

        public List<SelectListItem> Customers { get; set; }
            = new List<SelectListItem>();
    }
}