using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class MenuCategory
{
    public int CategoryId { get; set; }

    [Required, StringLength(100)]
    public string CategoryName { get; set; } = string.Empty; // Tên danh mục món ăn, ví dụ: "Khai vị", "Món chính", "Tráng miệng", v.v.
                                                             // không được nullable vì mỗi món ăn phải thuộc một danh mục cụ thể

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
