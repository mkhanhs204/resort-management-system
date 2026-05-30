using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class Review
{
    public int ReviewId { get; set; }
    public int? UserId { get; set; }
    public int? TourId { get; set; }
    public int? BookingId { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    public string? Comment { get; set; }
    public bool IsVisible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
    public Tour? Tour { get; set; }
    public Booking? Booking { get; set; }
}
