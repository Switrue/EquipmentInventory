namespace EquipmentInventory.API.Data.Models;

public record MemberDto(
    long Id,
    string Surname,
    string Username,
    string Position
);
