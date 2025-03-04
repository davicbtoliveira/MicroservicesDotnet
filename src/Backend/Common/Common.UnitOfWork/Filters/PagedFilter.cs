using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.UnitOfWork.Filters
{
    public class PagedFilter<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; private set; }
        public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public bool DisableTracking { get; private set; }
        public bool IgnoreQueryFilters { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public bool SplitQuery { get; private set; }

        public PagedFilter()
        {
            PageIndex = 0;
            PageSize = 20;
            DisableTracking = true;
            SplitQuery = false;
            IgnoreQueryFilters = false;
            CancellationToken = default;
        }

        public PagedFilter<TEntity> WithPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
            return this;
        }

        public PagedFilter<TEntity> WithOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            OrderBy = orderBy;
            return this;
        }

        public PagedFilter<TEntity> WithInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            Include = include;
            return this;
        }

        public PagedFilter<TEntity> WithPageIndex(int pageIndex)
        {
            PageIndex = pageIndex;
            return this;
        }

        public PagedFilter<TEntity> WithPageSize(int pageSize)
        {
            PageSize = pageSize;
            return this;
        }

        public PagedFilter<TEntity> WithDisableTracking(bool disableTracking)
        {
            DisableTracking = disableTracking;
            return this;
        }

        public PagedFilter<TEntity> WithSplitQuery(bool splitQuery)
        {
            SplitQuery = splitQuery;
            return this;
        }

        public PagedFilter<TEntity> WithIgnoreQueryFilters(bool ignoreQueryFilters)
        {
            IgnoreQueryFilters = ignoreQueryFilters;
            return this;
        }

        public PagedFilter<TEntity> WithCancellationToken(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }
    }
}
