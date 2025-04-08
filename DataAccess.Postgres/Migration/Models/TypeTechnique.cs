namespace DataAccess.Postgres.Migration.Models;

public partial class TypeTechnique
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Technique> Techniques { get; set; } = new List<Technique>();
}
