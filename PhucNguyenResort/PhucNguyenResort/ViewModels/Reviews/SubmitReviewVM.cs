using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Reviews;

public class SubmitReviewVM
{
    [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5")]
    public int? Rating { get; set; }

    [StringLength(2000, ErrorMessage = "Bình luận không vượt quá 2000 ký tự")]
    public string? Comment { get; set; }

    public int? BookingId { get; set; }   // Review phòng
    public int? TourId { get; set; }      // Review tour
    public int? TourBookingId { get; set; } // Specific tour booking
}
