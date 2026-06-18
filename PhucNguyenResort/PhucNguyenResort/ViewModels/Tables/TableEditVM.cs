using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Tables;

public class TableEditVM : TableCreateVM
{
    [Required]
    public int TableId { get; set; }
}
