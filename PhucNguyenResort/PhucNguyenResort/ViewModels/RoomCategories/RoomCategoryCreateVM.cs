using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PhucNguyenResort.ViewModels.RoomCategories
{
    public class RoomCategoryCreateVM
    {
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        [Range(1, 20)]
        public int? Capacity { get; set; }

        [Range(0, 999999999)]
        [Display(Name = "Price Per Night")]
        public decimal? PricePerNight { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Category Image")]
        public IFormFile? ImageUpload { get; set; }

        [Display(Name = "Gallery Images")]
        public List<IFormFile>? GalleryUploads { get; set; }
    }
}
