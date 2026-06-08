using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.MenuCategories;

public class MenuCategoryFormViewModel
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Category name")]
    public string CategoryName { get; set; } = string.Empty;
}
