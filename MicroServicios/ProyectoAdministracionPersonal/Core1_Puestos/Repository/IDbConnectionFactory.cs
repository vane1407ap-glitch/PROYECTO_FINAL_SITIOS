using System.Data;

namespace Core1_Puestos.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}