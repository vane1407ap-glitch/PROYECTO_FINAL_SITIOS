using System.Data;

namespace Core4_Login.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CrearConexion();
}