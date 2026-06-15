using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Tables;

public class TableCreateVM
{
    [Required(ErrorMessage = "Vui lòng nhập số hiệu bàn")]
    [StringLength(20, ErrorMessage = "Số hiệu bàn không vượt quá 20 ký tự")]
    [Display(Name = "Số bàn / Tên bàn")]
    public string? TableNumber { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập sức chứa")]
    [Range(1, 50, ErrorMessage = "Sức chứa từ 1 đến 50 người")]
    [Display(Name = "Sức chứa (người)")]
    public int? Capacity { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
    [Display(Name = "Trạng thái")]
    public DiningTableStatus Status { get; set; } = DiningTableStatus.Available;
}
