namespace DataAccess.Postgres.Migration.Models;

public partial class Archive
{
    public long Id { get; set; }

    public string TypeTechniqueName { get; set; } = null!;

    public string? MemberName { get; set; }

    public string? OfficeNumber { get; set; }

    public string TechniqueName { get; set; } = null!;

    public int? ComputerNumber { get; set; }

    public DateOnly DateOfPurchase { get; set; }

    public DateOnly WriteOffDate { get; set; }

    public DateOnly DateOfManufacture { get; set; }

    public bool UnderRepair { get; set; }

    public string Number { get; set; } = null!;

    public string Supplier { get; set; } = null!;

    public float Cost { get; set; }

    public DateOnly? DateOfUse { get; set; }
}
