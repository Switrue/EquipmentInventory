using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class LoginModel
{
    [Required]
    [StringLength(60)]
    public string Login { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 6, ErrorMessage = ApplicationErrors.IncorrectPasswordLength)]
    public string Password { get; set; }
}
