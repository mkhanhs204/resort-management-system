using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class MenuItem
{
    public int MenuItemId { get; set; }
    public int? CategoryId { get; set; }

    [StringLength(100)]
    public string? ItemName { get; set; }

    public string? Description { get; set; }

    [Range(0, 999999999)]
    public decimal? Price { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    public MenuCategory? Category { get; set; }
    public ICollection<FoodOrderItem> FoodOrderItems { get; set; } = new List<FoodOrderItem>();
}
