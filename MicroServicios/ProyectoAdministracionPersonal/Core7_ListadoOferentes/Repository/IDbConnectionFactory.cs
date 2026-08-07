using System.Data;

namespace Core7_ListadoOferentes.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}