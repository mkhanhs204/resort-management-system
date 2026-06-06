using System.ComponentModel.DataAnnotations;

using PhucNguyenResort.Models;

namespace PhucNguyenResort.ViewModels.RoomCategories
{
    public class RoomCategoryEditVM : RoomCategoryCreateVM
    {
        public int CategoryId { get; set; }

        public List<RoomCategoryImage>? ExistingGalleryImages { get; set; }
    }
}
