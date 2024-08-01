using System.Data;

namespace CKK.DB.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
