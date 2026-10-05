using System.ComponentModel.DataAnnotations;

namespace Vehicle_Service_Center.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        public ApplicationUser? Customer { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public Vehicle? Vehicle { get; set; }

        [Required]
        [StringLength(1000)]
        public string ServiceDescription { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? SparePartUsedDescription { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        public string? MechanicId { get; set; }

        public ApplicationUser? Mechanic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}