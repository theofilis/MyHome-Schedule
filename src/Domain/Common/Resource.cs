namespace MyHome.Domain.Common
{
    public abstract class Resource<T, K> : AuditableEntity<T> 
        where T: notnull where K: notnull
    {
        public K? Id { get; set; }
    }
}
