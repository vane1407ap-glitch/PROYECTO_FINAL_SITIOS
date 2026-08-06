using Core4_Login.Entities;
using Dapper;
using System.Data;

namespace Core4_Login.Repository;

public class UsuarioRepository
{
    private readonly IDbConnectionFactory
        _connectionFactory;

    public UsuarioRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Usuario?>
        ObtenerPorNombreUsuarioAsync(
            string nombreUsuario)
    {
        const string sql = """
            SELECT
                id_usuario AS IdUsuario,
                nombre_usuario AS NombreUsuario,
                nombre_completo AS NombreCompleto,
                contrasena AS Contrasena,
                intentos_fallidos AS IntentosFallidos,
                bloqueado AS Bloqueado,
                estado AS Estado,
                correo AS Correo,
                id_rol AS IdRol
            FROM usuarios
            WHERE TRIM(nombre_usuario) =
                  TRIM(@NombreUsuario)
            LIMIT 1;
            """;

        using IDbConnection conexion =
            _connectionFactory.CrearConexion();

        return await conexion
            .QueryFirstOrDefaultAsync<Usuario>(
                sql,
                new
                {
                    NombreUsuario = nombreUsuario
                });
    }

    public async Task RegistrarIntentoFallidoAsync(
        int idUsuario)
    {
        const string sql = """
            UPDATE usuarios
            SET
                intentos_fallidos =
                    intentos_fallidos + 1,

                bloqueado =
                    CASE
                        WHEN intentos_fallidos + 1 >= 3
                            THEN 1
                        ELSE bloqueado
                    END
            WHERE id_usuario = @IdUsuario;
            """;

        using IDbConnection conexion =
            _connectionFactory.CrearConexion();

        await conexion.ExecuteAsync(
            sql,
            new
            {
                IdUsuario = idUsuario
            });
    }

    public async Task ReiniciarIntentosAsync(
        int idUsuario)
    {
        const string sql = """
            UPDATE usuarios
            SET
                intentos_fallidos = 0,
                bloqueado = 0
            WHERE id_usuario = @IdUsuario;
            """;

        using IDbConnection conexion =
            _connectionFactory.CrearConexion();

        await conexion.ExecuteAsync(
            sql,
            new
            {
                IdUsuario = idUsuario
            });
    }

    public async Task<Usuario?>
        ObtenerPorIdAsync(int idUsuario)
    {
        const string sql = """
            SELECT
                id_usuario AS IdUsuario,
                nombre_usuario AS NombreUsuario,
                nombre_completo AS NombreCompleto,
                contrasena AS Contrasena,
                intentos_fallidos AS IntentosFallidos,
                bloqueado AS Bloqueado,
                estado AS Estado,
                correo AS Correo,
                id_rol AS IdRol
            FROM usuarios
            WHERE id_usuario = @IdUsuario
            LIMIT 1;
            """;

        using IDbConnection conexion =
            _connectionFactory.CrearConexion();

        return await conexion
            .QueryFirstOrDefaultAsync<Usuario>(
                sql,
                new
                {
                    IdUsuario = idUsuario
                });
    }
}