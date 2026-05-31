using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class UserProfile
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(50)]
    public string? Nationality { get; set; }

    public byte[]? PassportNumber { get; set; }

    [StringLength(255)]
    public string? PassportImage { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    public bool ConsentGiven { get; set; } = false;
    public bool DeletionRequested { get; set; } = false;

    public User? User { get; set; }
}
