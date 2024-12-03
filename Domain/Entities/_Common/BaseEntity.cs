namespace Talabat.Core.Domain.Entities._Common
{
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
