using DataAccess.Postgres.Migration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EquipmentInventory.API.Helpers;

public class EntityValidator
{
    private readonly EquipmentInventoryDbContext _context;

    public EntityValidator(EquipmentInventoryDbContext context)
        => _context = context;

    public async Task<bool> ExistsAsync<TEntity>(long id) where TEntity : class
        => await _context.Set<TEntity>().AnyAsync(e => EF.Property<long>(e, "Id") == id);

    public async Task<bool> ExistsOptionalAsync<TEntity>(long? id) where TEntity : class
        => !id.HasValue || await ExistsAsync<TEntity>(id.Value);

    public async Task<bool> ValidateReferenceAsync<TEntity>(long? id) where TEntity : class
        => !id.HasValue || await ExistsAsync<TEntity>(id.Value);

    public async Task<bool> IsUniqueAsync<TEntity, TProperty>(
        DbSet<TEntity> dbSet,
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value,
        long? excludeId = null)
        where TEntity : class
    {
        var parameter = Expression.Parameter(typeof(TEntity), "t");

        // Извлекаем MemberExpression, игнорируя Convert
        var memberExpression = propertySelector.Body is UnaryExpression unary
            ? unary.Operand as MemberExpression
            : propertySelector.Body as MemberExpression;

        if (memberExpression == null)
            throw new ArgumentException("Invalid property selector");

        // Создаем условие для проверки свойства
        var propertyCheck = Expression.Equal(
            Expression.Property(parameter, memberExpression.Member.Name),
            Expression.Constant(value, typeof(TProperty)));

        Expression condition = propertyCheck;

        // Добавляем условие исключения по ID
        if (excludeId.HasValue)
        {
            var idCheck = Expression.NotEqual(
                Expression.Property(parameter, "Id"),
                Expression.Constant(excludeId.Value, typeof(long)));

            condition = Expression.AndAlso(propertyCheck, idCheck);
        }

        var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
        return !await dbSet.AnyAsync(lambda);
    }
}
