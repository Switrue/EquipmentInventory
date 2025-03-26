using System;
using System.Linq;

namespace EquipmentInventory.Classes.Data
{
    class PasswordGenerator
    {
        public static string Get()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, random.Next(5, 15))
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
