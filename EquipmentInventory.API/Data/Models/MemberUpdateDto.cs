using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class MemberUpdateDto
{
    [StringLength(60)]
    public string? Username { get; set; }

    [StringLength(60)]
    public string? Surname { get; set; }
    public long? IdPosition { get; set; }
}
