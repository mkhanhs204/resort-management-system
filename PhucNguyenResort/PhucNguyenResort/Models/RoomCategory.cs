using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class RoomCategory
{
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    [Range(1, 20)]
    public int? MaxAdults { get; set; }

    [Range(0, 10)]
    public int? MaxChildren { get; set; }

    [Range(0, 999999999)]
    public decimal? PricePerNight { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<RoomCategoryImage> Images { get; set; } = new List<RoomCategoryImage>();
}
