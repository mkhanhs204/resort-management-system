using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhucNguyenResort.ViewModels.RestaurantMenus;

public class MenuItemFormViewModel : IValidatableObject
{
    public int MenuItemId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Item name")]
    public string ItemName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Range(0, 999999999)]
    public decimal Price { get; set; }

    [StringLength(255)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Category")]
    public int? CategoryId { get; set; }

    [ValidateNever]
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsAvailable && !CategoryId.HasValue)
        {
            yield return new ValidationResult(
                "Available menu items must be assigned to a category.",
                new[] { nameof(CategoryId) });
        }
    }
}
