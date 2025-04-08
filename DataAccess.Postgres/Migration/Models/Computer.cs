namespace DataAccess.Postgres.Migration.Models;

public partial class Computer
{
    public long Id { get; set; }

    public int Number { get; set; }

    public string PowerSupply { get; set; } = null!;

    public string Motherboard { get; set; } = null!;

    public string Ram { get; set; } = null!;

    public string Cpu { get; set; } = null!;

    public string Os { get; set; } = null!;

    public string? VideoCard { get; set; }

    public virtual Technique? Technique { get; set; }
}
