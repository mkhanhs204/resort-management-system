using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class Tour
{
    public int TourId { get; set; }

    [StringLength(100)]
    public string? TourName { get; set; }

    public string? Description { get; set; }

    [Range(0, 999999999)]
    public decimal? Price { get; set; }

    [Range(1, 1000)]
    public int? MaxCapacity { get; set; }

    [Range(1, 1000)]
    public int? DurationHours { get; set; }

    [StringLength(255)]
    public string? Location { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    public ICollection<TourSchedule> TourSchedules { get; set; } = new List<TourSchedule>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
