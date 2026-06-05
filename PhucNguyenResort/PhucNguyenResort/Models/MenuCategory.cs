using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class MenuCategory
{
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string? CategoryName { get; set; }

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
