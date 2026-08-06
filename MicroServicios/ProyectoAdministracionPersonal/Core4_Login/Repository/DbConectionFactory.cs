using MySqlConnector;
using System.Data;

namespace Core4_Login.Repository;

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
                "No se encontró la conexión DefaultConnection.");
    }

    public IDbConnection CrearConexion()
    {
        return new MySqlConnection(
            _connectionString);
    }
}