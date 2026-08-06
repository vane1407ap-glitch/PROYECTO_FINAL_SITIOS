using MySqlConnector;
using System.Data;

namespace Core3_Empleados.Repository;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(
        IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString(
                "DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se configuró la conexión a la base de datos.");
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(
            _connectionString);
    }
}