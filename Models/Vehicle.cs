using System.ComponentModel.DataAnnotations;

namespace Vehicle_Service_Center.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string VehicleNumberPlate { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string VehicleBrand { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string VehicleModel { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Issue { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        public ApplicationUser? Customer { get; set; }
    }
}