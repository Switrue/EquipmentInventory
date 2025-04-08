namespace DataAccess.Postgres.Migration.Models;

public partial class Technique
{
    public long Id { get; set; }

    public long IdTypeTechnique { get; set; }

    public long? IdMember { get; set; }

    public long? IdOffice { get; set; }

    public string Name { get; set; } = null!;

    public long? IdComputer { get; set; }

    public DateOnly DateOfPurchase { get; set; }

    public bool UnderRepair { get; set; }

    public DateOnly DateOfManufacture { get; set; }

    public string Number { get; set; } = null!;

    public long IdSupplier { get; set; }

    public float Cost { get; set; }

    public DateOnly? DateOfUse { get; set; }

    public virtual Computer? IdComputerNavigation { get; set; }

    public virtual Member? IdMemberNavigation { get; set; }

    public virtual Office? IdOfficeNavigation { get; set; }

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;

    public virtual TypeTechnique IdTypeTechniqueNavigation { get; set; } = null!;
}
