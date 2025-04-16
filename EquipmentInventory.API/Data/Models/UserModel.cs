using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class UserModel
{
    [StringLength(60)]
    public string? Username { get; set; }

    [StringLength(60)]
    public string? Surname { get; set; }

    [StringLength(60, MinimumLength = 6, ErrorMessage = ApplicationErrors.IncorrectPasswordLength)]
    public string? Password { get; set; }
    public byte[]? Image { get; set; }
}
