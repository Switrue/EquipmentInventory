using EquipmentInventory.Classes.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace EquipmentInventory.Classes.Services;

public static class ApiService
{
        public static Users ExtractUserFromJwt(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(jwt);

        foreach (var claim in jwtToken.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        string GetClaimValue(string claimType)
        {
            return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value
                ?? throw new ArgumentException($"Missing {claimType} claim");
        }

        return new Users
        {
            Id = long.Parse(GetClaimValue("nameid")),
            Username = GetClaimValue("unique_name"),
            Surname = GetClaimValue("family_name"),
            Role = GetClaimValue("role"),
            Image = GetImageBytes(jwtToken.Claims.FirstOrDefault(c => c.Type == "image")?.Value)
        };
    }

    private static byte[] GetImageBytes(string base64Image)
    {
        if (string.IsNullOrEmpty(base64Image)) return null;

        try
        {
            return Convert.FromBase64String(base64Image);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Ошибка конвертации изображения: {ex.Message}");
            return null;
        }
    }
}
