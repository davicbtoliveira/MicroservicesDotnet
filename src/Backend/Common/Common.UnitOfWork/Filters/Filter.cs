using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.UnitOfWork.Filters
{
    public class Filter<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; private set; }
        public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include { get; private set; }
        public bool DisableTracking { get; private set; }
        public bool IgnoreQueryFilters { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public Filter()
        {
            DisableTracking = true;
            IgnoreQueryFilters = false;
            CancellationToken = default;
        }

        public Filter<TEntity> WithPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
            return this;
        }

        public Filter<TEntity> WithOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            OrderBy = orderBy;
            return this;
        }

        public Filter<TEntity> WithInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            Include = include;
            return this;
        }

        public Filter<TEntity> WithDisableTracking(bool disableTracking)
        {
            DisableTracking = disableTracking;
            return this;
        }

        public Filter<TEntity> WithIgnoreQueryFilters(bool ignoreQueryFilters)
        {
            IgnoreQueryFilters = ignoreQueryFilters;
            return this;
        }

        public Filter<TEntity> WithCancellationToken(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }
    }
}
