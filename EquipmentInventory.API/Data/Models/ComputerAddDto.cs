using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class ComputerAddDto
{
    [Required]
    public int Number { get; set; }

    [Required] 
    [StringLength(200)] 
    public string PowerSupply { get; set; }

    [Required] 
    [StringLength(200)] 
    public string Motherboard { get; set; }

    [Required] 
    [StringLength(200)] 
    public string Ram { get; set; }

    [Required] 
    [StringLength(200)] 
    public string Cpu { get; set; }

    [Required] 
    [StringLength(200)] 
    public string Os { get; set; }
    public string? VideoCard { get; set; }
}
