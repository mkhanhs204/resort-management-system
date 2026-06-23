using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Tours
{
    public class TourCreateVM
    {
        [Required(ErrorMessage = "Tour Name is required")]
        [StringLength(100, ErrorMessage = "Tour Name cannot exceed 100 characters")]
        public string TourName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 999999999, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Max Capacity is required")]
        [Range(1, 1000, ErrorMessage = "Max Capacity must be between 1 and 1000")]
        public int MaxCapacity { get; set; }

        [Required(ErrorMessage = "Duration (Hours) is required")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 hours")]
        public int DurationHours { get; set; }

        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters")]
        public string? Location { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public int? ManagerId { get; set; }
    }
}
