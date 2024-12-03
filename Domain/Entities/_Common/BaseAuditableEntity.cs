namespace Talabat.Core.Domain.Entities._Common
{
    public interface IBaseAuditableEntity
    {
        string CreatedBy { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        string lastModifedBy { get; set; }
        DateTimeOffset lastModifedOn { get; set; }
    }

    public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IBaseAuditableEntity where TKey : IEquatable<TKey>
    {
        public string CreatedBy { get; set; } = null!;
        public DateTimeOffset CreatedOn { get; set; }
        public string lastModifedBy { get; set ; } = null!;
        public DateTimeOffset lastModifedOn { get ; set; }
    }
}
