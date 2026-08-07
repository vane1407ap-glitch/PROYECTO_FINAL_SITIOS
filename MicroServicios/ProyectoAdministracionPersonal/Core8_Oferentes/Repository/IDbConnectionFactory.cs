using System.Data;

namespace Core8_Oferentes.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}