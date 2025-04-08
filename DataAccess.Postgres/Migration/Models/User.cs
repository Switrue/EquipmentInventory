namespace DataAccess.Postgres.Migration.Models;

public partial class User
{
    public long Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public long IdRole { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Image { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
