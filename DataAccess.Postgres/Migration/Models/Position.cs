namespace DataAccess.Postgres.Migration.Models;

public partial class Position
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
