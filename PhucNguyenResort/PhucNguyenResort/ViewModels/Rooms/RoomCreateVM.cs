using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Rooms
{
    public class RoomCreateVM
    {
        [Required(ErrorMessage = "Room Number is required")]
        [StringLength(20)]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = null!;

        [Required(ErrorMessage = "Please select a Category")]
        [Display(Name = "Room Category")]
        public int CategoryId { get; set; }

        [Range(0, 200)]
        [Display(Name = "Floor Number")]
        public int? FloorNumber { get; set; }

        public IEnumerable<SelectListItem>? CategoriesList { get; set; }
    }
}
