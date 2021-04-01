using MyHome.Domain.Common;
using System.Linq;

namespace MyHome.Domain.Extensions
{
    public static class OwnableExtensions
    {
        public static IQueryable<TSource> OwnedBy<TSource, TId>(this IQueryable<TSource> source,
                                                              TId ownerId) where TSource : AuditableEntity<TId>
        {
            if (ownerId == null) return source;

            return source
                 .Where(c => Equals(c.OwnerId, ownerId));
        }
    }
}
