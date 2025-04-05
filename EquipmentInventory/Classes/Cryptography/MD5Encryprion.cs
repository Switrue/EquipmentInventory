using System;
using System.Security.Cryptography;
using System.Text;

namespace EquipmentInventory.Classes.Cryptography;

public class MD5Encryprion
{
    public static string GetMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    public static bool ComparePasswords(string password, string hash)
    {
        string hashedPassword = GetMD5Hash(password);
        return hashedPassword.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}
