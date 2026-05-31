using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class Role
{
    public int RoleId { get; set; }

    [Required, StringLength(50)]
    public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}
