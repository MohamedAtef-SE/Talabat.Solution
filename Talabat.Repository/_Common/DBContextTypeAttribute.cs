namespace Talabat.Infrastructure.Persistence._Common
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class DBContextTypeAttribute : Attribute
    {
        public Type DBType { get; set; }
        public DBContextTypeAttribute(Type dBType)
        {
            DBType = dBType;
        }


    }
}
