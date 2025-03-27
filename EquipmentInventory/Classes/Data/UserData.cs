using EquipmentInventory.Classes.Data.Database;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using Npgsql;

namespace EquipmentInventory.Classes.Data
{
    public class UserData
    {
        public long UserId { get; private set; }
        public string UserRole { get; private set; }
        public string Username { get; private set; }
        public string Surname { get; private set; }

        public UserData(long userId)
        {
            UserId = userId;
            GetUserData();
        }

        public void GetUserData()
        {
            try
            {
                var userData = ConnectionDatabase.ExecuteQuery(
                    "select r.name, username, surname from users u join roles r on r.id = u.id_role where u.id = @id", 
                    new NpgsqlParameter("@id", UserId)
                ).Rows[0];

                UserRole = userData[0].ToString();
                Username = userData[1].ToString();
                Surname = userData[2].ToString();
            }
            catch 
            {
                CustomMessageBoxHelper.Show(Strings.Error, Strings.DatabaseError, false);
            }
        }
    }
}
