using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EquipmentInventory.API.Services;

public class MembersService
{
    private readonly EquipmentInventoryDbContext _context;
    private readonly EntityValidator _entityValidator;

    public MembersService(
        EquipmentInventoryDbContext context,
        EntityValidator entityValidator)
    {
        _context = context;
        _entityValidator = entityValidator;
    }

    #region Get
    private IQueryable<Member> GetQuery()
    {
        return _context.Members
            .AsNoTracking()
            .Include(m => m.IdPositionNavigation)
            .OrderBy(m => m.Id);
    }

    public async Task<IEnumerable<MemberDto>> GetPaginatedResults(PaginationModel pagination)
    {
        var query = GetQuery();

        if (pagination.Page.HasValue && pagination.PageSize.HasValue)
        {
            query = query
                .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                .Take(pagination.PageSize.Value);
        }

        var items = await query
            .Select(c => new MemberDto(
                c.Id,
                c.Surname,
                c.Username,
                c.IdPositionNavigation.Name
            ))
            .ToListAsync();

        return items;
    }
    #endregion

    #region Add
    public async Task<Member> CreateMemberAsync(MemberAddDto model)
    {
        var member = new Member
        {
            Username = model.Username,
            Surname = model.Surname,
            IdPosition = model.IdPosition
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }
    #endregion

    #region Update
    public void UpdateMember(Member existing, MemberUpdateDto model)
    {
        existing.Username = model.Username ?? existing.Username;
        existing.Surname = model.Surname ?? existing.Surname;
        existing.IdPosition = model.IdPosition ?? existing.IdPosition;
    }
    #endregion

    #region Validation Helpers
    public async Task<ActionResult> ValidateAddModelAsync(MemberAddDto model)
    {
        var validations = new List<(bool Result, string Error)>
        {
            (await _entityValidator.ExistsAsync<Position>(model.IdPosition),
                ApplicationErrors.IncorrectPosition)
        };

        var error = validations.FirstOrDefault(v => !v.Result).Error;
        return error != null ? ApiResponseHelper.BadRequest(error) : null;
    }

    public async Task<ActionResult> ValidateUpdateModelAsync(MemberUpdateDto model)
    {
        var validations = new List<(bool Result, string Error)>
        {
            (await _entityValidator.ValidateReferenceAsync<Position>(model.IdPosition),
                ApplicationErrors.IncorrectPosition)
        };

        var error = validations.FirstOrDefault(v => !v.Result).Error;
        return error != null ? ApiResponseHelper.BadRequest(error) : null;
    }
    #endregion
}
