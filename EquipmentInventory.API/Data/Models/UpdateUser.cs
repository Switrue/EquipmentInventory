using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class UpdateUser
{
    [StringLength(60)]
    public string? Username { get; set; }

    [StringLength(60)]
    public string? Surname { get; set; }

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль не может быть меньше 6 символов")]
    public string? Password { get; set; }

    public byte[]? Image { get; set; }
}
