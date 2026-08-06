using System.Data;

namespace Core3_Empleados.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
} 