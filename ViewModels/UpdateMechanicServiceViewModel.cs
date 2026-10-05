using System.ComponentModel.DataAnnotations;

namespace Vehicle_Service_Center.ViewModels
{
    public class UpdateMechanicServiceViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Service Description")]
        public string ServiceDescription { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Spare Part Used Description")]
        public string? SparePartUsedDescription { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";
    }
}