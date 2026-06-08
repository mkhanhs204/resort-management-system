using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.Models;

public class RoomCategoryImage
{
    [Key]
    public int ImageId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string ImageUrl { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public RoomCategory? Category { get; set; }
}
