using System.Collections.Generic;

namespace EquipmentInventory.Classes.Data.Database
{
    internal class TokenValidation
    {
        private static readonly HashSet<string> InvalidTokens = new HashSet<string>
        {
            "no_tokens_found",
            "refresh_token_expired"
        };

        public static bool IsTokenInvalid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            return InvalidTokens.Contains(token);
        }
    }
}
