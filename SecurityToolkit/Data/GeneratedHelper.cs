using System.Security.Cryptography;
using System.Text;

namespace SecurityToolkit.Data
{
    public static class GeneratedHelper
    {
        private static readonly Random _random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // 32 байта = 256 бит
        public static string GenerateRandomKey(int size = 32) 
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[size];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }

        public static string GenerateRandomCode()
        {
            var stringBuilder = new StringBuilder(19);
            for (int i = 0; i < 4; i++)
            {
                stringBuilder.Append(GenerateRandomSegment());
                if (i < 3)
                {
                    stringBuilder.Append('-');
                }
            }
            return stringBuilder.ToString();
        }

        private static string GenerateRandomSegment()
        {
            var segmentBuilder = new StringBuilder(4);
            for (int i = 0; i < 4; i++)
            {
                segmentBuilder.Append(Characters[_random.Next(Characters.Length)]);
            }
            return segmentBuilder.ToString();
        }
    }
}
