using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Tables;

public class TableCreateVM
{
    [Required(ErrorMessage = "Vui lòng nhập số hiệu bàn")]
    [StringLength(20, ErrorMessage = "Số hiệu bàn không vượt quá 20 ký tự")]
    [Display(Name = "Số bàn / Tên bàn")]
    public string? TableNumber { get; set; }

    [StringLength(100)]
    [Display(Name = "Table display name")]
    public string TableName { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [StringLength(50)]
    [Display(Name = "Area")]
    public string? Area { get; set; }

    [StringLength(50)]
    [Display(Name = "Table type")]
    public string? TableType { get; set; }

    [StringLength(150)]
    [Display(Name = "Location note")]
    public string? LocationNote { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập sức chứa")]
    [Range(1, 50, ErrorMessage = "Sức chứa từ 1 đến 50 người")]
    [Display(Name = "Sức chứa (người)")]
    public int? Capacity { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
    [Display(Name = "Trạng thái")]
    public DiningTableStatus Status { get; set; } = DiningTableStatus.Available;

    [Display(Name = "Reservable online")]
    public bool IsReservable { get; set; } = true;

    [Display(Name = "Display order")]
    public int DisplayOrder { get; set; } = 0;
}
