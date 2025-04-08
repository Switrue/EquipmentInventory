namespace DataAccess.Postgres.Migration.Models;

public partial class Member
{
    public long Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public long IdPosition { get; set; }

    public virtual Position IdPositionNavigation { get; set; } = null!;

    public virtual ICollection<Technique> Techniques { get; set; } = new List<Technique>();
}
