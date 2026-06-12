using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

// IsAvailable dùng cho trạng thái bán tạm thời; IsDeleted dùng khi món đã ngừng kinh doanh.
public class MenuItem
{
    public int MenuItemId { get; set; }
    public int? CategoryId { get; set; }

    [Required, StringLength(100)]
    public string ItemName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(0, 999999999)]
    public decimal Price { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsDeleted { get; set; } = false;

    public MenuCategory? Category { get; set; }
    public ICollection<FoodOrderItem> FoodOrderItems { get; set; } = new List<FoodOrderItem>();
}
