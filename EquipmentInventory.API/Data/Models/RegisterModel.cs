using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class RegisterModel
{
    [Required]
    [StringLength(60)]
    public string Username { get; set; }

    [Required]
    [StringLength(60)]
    public string Surname { get; set; }

    [Required]
    [StringLength(60)]
    public string Login { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль не может быть меньше 6 символов")]
    public string Password { get; set; }

    public byte[]? Image { get; set; }
}
