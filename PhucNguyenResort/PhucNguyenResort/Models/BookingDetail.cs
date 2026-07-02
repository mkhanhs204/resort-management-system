using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.Models;

public class BookingDetail
{
    public int BookingDetailId { get; set; }
    
    public int BookingId { get; set; }
    
    public int RoomCategoryId { get; set; }
    
    public int? RoomId { get; set; }
    
    [Range(0, 999999999)]
    public decimal? Price { get; set; }

    public Booking? Booking { get; set; }
    public RoomCategory? RoomCategory { get; set; }
    public Room? Room { get; set; }
}
