using Core3_Empleados.Entities;
using Dapper;

namespace Core3_Empleados.Repository;

public class EmpleadoRepository
    : IEmpleadoRepository
{
    private readonly IDbConnectionFactory
        _connectionFactory;

    public EmpleadoRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory =
            connectionFactory;
    }

    public async Task<bool>
        ExisteNumeroEmpleadoAsync(
            string numeroEmpleado)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM empleados
            WHERE numero_empleado = @NumeroEmpleado;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        int cantidad =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    NumeroEmpleado =
                        numeroEmpleado
                });

        return cantidad > 0;
    }

    public async Task<bool>
        ExisteIdentificacionAsync(
            string identificacion)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM empleados
            WHERE identificacion = @Identificacion;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        int cantidad =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    Identificacion =
                        identificacion
                });

        return cantidad > 0;
    }

    public async Task<bool>
        ExisteCorreoAsync(
            string correo)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM empleados
            WHERE correo = @Correo;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        int cantidad =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    Correo = correo
                });

        return cantidad > 0;
    }

    public async Task<bool>
        ExisteTelefonoAsync(
            string telefono)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM empleados
            WHERE telefono = @Telefono;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        int cantidad =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    Telefono = telefono
                });

        return cantidad > 0;
    }

    public async Task<bool>
        ExistePuestoAsync(
            int idPuesto)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM puestos
            WHERE id = @IdPuesto;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        int cantidad =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    IdPuesto = idPuesto
                });

        return cantidad > 0;
    }

    public async Task<EmpleadoDTO>
        CrearEmpleadoAsync(
            CrearEmpleadoRequest request,
            DateTime fechaNacimiento,
            DateTime fechaContratacion)
    {
        const string insertarSql = """
            INSERT INTO empleados
            (
                numero_empleado,
                identificacion,
                tipo_identificacion,
                nombre_completo,
                fecha_nacimiento,
                correo,
                telefono,
                id_puesto,
                fecha_contratacion,
                estado
            )
            VALUES
            (
                @NumeroEmpleado,
                @Identificacion,
                @TipoIdentificacion,
                @NombreCompleto,
                @FechaNacimiento,
                @Correo,
                @Telefono,
                @IdPuesto,
                @FechaContratacion,
                @Estado
            );

            SELECT LAST_INSERT_ID();
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        int idEmpleado =
            await connection.ExecuteScalarAsync<int>(
                insertarSql,
                new
                {
                    request.NumeroEmpleado,
                    request.Identificacion,
                    request.TipoIdentificacion,
                    request.NombreCompleto,
                    FechaNacimiento =
                        fechaNacimiento,
                    request.Correo,
                    request.Telefono,
                    request.IdPuesto,
                    FechaContratacion =
                        fechaContratacion,
                    request.Estado
                });

        return new EmpleadoDTO
        {
            IdEmpleado = idEmpleado,
            NumeroEmpleado =
                request.NumeroEmpleado,
            Identificacion =
                request.Identificacion,
            TipoIdentificacion =
                request.TipoIdentificacion,
            NombreCompleto =
                request.NombreCompleto,
            FechaNacimiento =
                fechaNacimiento.ToString(
                    "yyyy-MM-dd"),
            Correo = request.Correo,
            Telefono = request.Telefono,
            IdPuesto = request.IdPuesto,
            FechaContratacion =
                fechaContratacion.ToString(
                    "yyyy-MM-dd"),
            Estado = request.Estado
        };
    }
}