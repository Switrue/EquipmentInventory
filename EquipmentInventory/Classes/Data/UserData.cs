using EquipmentInventory.Classes.Data.Database;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using Npgsql;
using System;

namespace EquipmentInventory.Classes.Data
{
    public class UserData
    {
        public long UserId { get; private set; }

        public long UserRoleId { get; private set; }

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
                    "select id_role, username, surname from users where id = @id", 
                    new NpgsqlParameter("@id", UserId)
                ).Rows[0];

                UserRoleId = Convert.ToInt64(userData[0]);
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
