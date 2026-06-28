namespace PhucNguyenResort.ViewModels.Reviews;

public class ReviewListVM
{
    public int ReviewId { get; set; }
    public string? UserName { get; set; }
    public int? Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVisible { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ServiceType { get; set; }  // "Room" | "Tour"
    public string? ServiceName { get; set; }  // Tên Tour hoặc Room Category
}
