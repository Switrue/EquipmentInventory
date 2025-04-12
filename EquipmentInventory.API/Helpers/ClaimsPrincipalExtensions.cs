using System.Security.Claims;

namespace EquipmentInventory.API.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out long userId)
    {
        userId = 0;
        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return !string.IsNullOrEmpty(userIdValue) && long.TryParse(userIdValue, out userId);
    }
}
