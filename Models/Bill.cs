using System.ComponentModel.DataAnnotations;

namespace Vehicle_Service_Center.Models
{
    public class Bill
    {
        public int Id { get; set; }

        [Required]
        public int ServiceRequestId { get; set; }

        public ServiceRequest? ServiceRequest { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        public ApplicationUser? Customer { get; set; }

        [Required]
        [StringLength(20)]
        public string VehicleNumberPlate { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string ServiceDescription { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal SparePartCharge { get; set; }

        [Range(0, double.MaxValue)]
        public decimal LabourCharge { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentType { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = "Pending";

        public decimal TotalAmount =>
            SparePartCharge + LabourCharge;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}