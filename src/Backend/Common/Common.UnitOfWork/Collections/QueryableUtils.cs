using Microsoft.EntityFrameworkCore;

namespace Common.UnitOfWork.Collections
{
    public static class QueryableUtils
    {
        public static IOrderedQueryable<T> OrderByColumnName<T>(this IQueryable<T> queryableElement, string columnName, bool? IsAsc = true)
        {
            return (IsAsc.Value) ? queryableElement.OrderBy(p => EF.Property<object>(p, columnName)) :
                                   queryableElement.OrderByDescending(p => EF.Property<object>(p, columnName));
        }
    }
}
