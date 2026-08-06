using Core1_Puestos.Entities;
using Dapper;

namespace Core1_Puestos.Repository;

public class PuestoRepository : IPuestoRepository
{
    private readonly IDbConnectionFactory
        _connectionFactory;

    public PuestoRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory =
            connectionFactory;
    }

    public async Task<IEnumerable<PuestoDTO>>
        ObtenerPuestosActivosAsync(
            int pagina,
            int tamanoPagina)
    {
        int desplazamiento =
            (pagina - 1) * tamanoPagina;

        const string sql = """
            SELECT
                codigo_puesto AS CodigoPuesto,
                nombre_puesto AS NombrePuesto
            FROM puestos
            WHERE estado = 'Activo'
            ORDER BY nombre_puesto ASC
            LIMIT @TamanoPagina
            OFFSET @Desplazamiento;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QueryAsync<PuestoDTO>(
            sql,
            new
            {
                TamanoPagina = tamanoPagina,
                Desplazamiento = desplazamiento
            });
    }

    public async Task<int>
        ContarPuestosActivosAsync()
    {
        const string sql = """
            SELECT COUNT(*)
            FROM puestos
            WHERE estado = 'Activo';
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            sql);
    }
}