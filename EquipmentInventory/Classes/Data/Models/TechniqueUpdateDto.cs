using Newtonsoft.Json;
using System;

namespace EquipmentInventory.Classes.Data.Models;

public class TechniqueUpdateDto
{
    public string? Number { get; set; }
    public long? IdTypeTechnique { get; set; }
    public string? Name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? IdMember { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? IdOffice { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? IdComputer { get; set; }
    public DateTime? DateOfPurchase { get; set; }
    public DateTime? DateOfManufacture { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? DateOfUse { get; set; }
    public long? IdSupplier { get; set; }
    public float? Cost { get; set; } = 0;
    public bool? UnderRepair { get; set; } = false;

    public bool IsValid()
    {
        return HasRequiredFields() && AreDatesValid();
    }

    private bool HasRequiredFields()
    {
        return !string.IsNullOrWhiteSpace(Number) &&
               IdTypeTechnique.HasValue &&
               !string.IsNullOrWhiteSpace(Name) &&
               DateOfPurchase.HasValue &&
               DateOfManufacture.HasValue &&
               IdSupplier.HasValue &&
               Cost.HasValue;
    }

    private bool AreDatesValid()
    {
        return IsDateValid(DateOfPurchase) &&
               IsDateValid(DateOfManufacture) &&
               IsDateValid(DateOfUse);
    }

    private bool IsDateValid(DateTime? date)
    {
        if (!date.HasValue) return true;

        return date.Value != DateTime.MinValue;
    }
}
