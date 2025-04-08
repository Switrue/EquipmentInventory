namespace DataAccess.Postgres.Migration.Models;

public partial class Office
{
    public long Id { get; set; }

    public int Floor { get; set; }

    public int? Number { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Technique> Techniques { get; set; } = new List<Technique>();
}
