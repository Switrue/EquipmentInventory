using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class MemberAddDto
{
    [Required]
    [StringLength(60)]
    public string Username { get; set; }

    [Required]
    [StringLength(60)]
    public string Surname { get; set; }

    [Required]
    public long IdPosition { get; set; }
}
