using System;

namespace MyHome.Domain.Common
{
    public abstract class AuditableEntity<T>
    {
        public T? CreatedBy { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public T? LastModifiedBy { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;   

        public T? OwnerId { get; set; }
    }
}
