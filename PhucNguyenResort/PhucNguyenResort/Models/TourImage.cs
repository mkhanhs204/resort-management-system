using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.Models;

public class TourImage
{
    [Key]
    public int ImageId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string ImageUrl { get; set; } = null!;
    
    public int TourId { get; set; }
    public Tour? Tour { get; set; }
}
