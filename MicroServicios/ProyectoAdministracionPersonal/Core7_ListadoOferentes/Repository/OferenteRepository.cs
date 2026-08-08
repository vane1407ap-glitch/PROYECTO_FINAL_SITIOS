using Core7_ListadoOferentes.Entities;
using Dapper;
using System.Data;

namespace Core7_ListadoOferentes.Repository;

public class OferenteRepository : IOferenteRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public OferenteRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> ObtenerIdPuestoPorCodigoAsync(string codigoPuesto)
    {
        const string sql = """
            SELECT id
            FROM puestos
            WHERE codigo_puesto = @CodigoPuesto
            """;

        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.ExecuteScalarAsync<int?>(sql, new { CodigoPuesto = codigoPuesto });

        if (!result.HasValue)
        {
            throw new Exception("Puesto no encontrado");
        }

        return result.Value;
    }

    public async Task<IEnumerable<OferenteResumenDTO>> ObtenerOferentesPorIdPuestoAsync(int idPuesto)
    {
        const string sql = """
            SELECT
                pp.id_postulacion AS IdPostulacion,
                o.codigo_oferente AS CodigoOferente,
                o.identificacion AS Identificacion,
                o.nombre_completo AS Nombre,
                '' AS Apellido,
                o.correo AS Email,
                o.telefono AS Telefono,
                pp.curriculum AS Curriculum,
                pp.fecha_postulacion AS FechaPostulacion
            FROM postulaciones_puestos pp
            INNER JOIN oferentes o
                ON pp.identificacion = o.identificacion
            WHERE pp.id_puesto = @IdPuesto
            ORDER BY pp.fecha_postulacion DESC
            """;

        using var connection = _connectionFactory.CreateConnection();

        var oferentes = await connection.QueryAsync<OferenteResumenDTO>(sql, new { IdPuesto = idPuesto });

        // Formatear fechas
        foreach (var oferente in oferentes)
        {
            if (!string.IsNullOrEmpty(oferente.FechaPostulacion))
            {
                if (DateTime.TryParse(oferente.FechaPostulacion, out DateTime fecha))
                {
                    oferente.FechaPostulacion = fecha.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        return oferentes;
    }

    public async Task<OferenteDetalleDTO?> ObtenerDetalleOferenteAsync(int idPostulacion)
    {
        const string sql = """
            SELECT
                pp.id_postulacion AS IdPostulacion,
                o.identificacion AS Identificacion,
                o.nombre_completo AS Nombre,
                '' AS Apellido,
                o.correo AS Email,
                o.telefono AS Telefono,
                '' AS Direccion,
                o.fecha_nacimiento AS FechaNacimiento,
                pp.curriculum AS Curriculum,
                pp.fecha_postulacion AS FechaPostulacion,
                p.nombre_puesto AS NombrePuesto,
                p.codigo_puesto AS CodigoPuesto,
                p.salario AS Salario,
                p.estado AS EstadoPuesto
            FROM postulaciones_puestos pp
            INNER JOIN oferentes o
                ON pp.identificacion = o.identificacion
            INNER JOIN puestos p
                ON pp.id_puesto = p.id
            WHERE pp.id_postulacion = @IdPostulacion
            """;

        using var connection = _connectionFactory.CreateConnection();

        var oferente = await connection.QueryFirstOrDefaultAsync<OferenteDetalleDTO>(sql, new { IdPostulacion = idPostulacion });

        if (oferente != null)
        {
            // Formatear fechas
            if (!string.IsNullOrEmpty(oferente.FechaNacimiento))
            {
                if (DateTime.TryParse(oferente.FechaNacimiento, out DateTime fecha))
                {
                    oferente.FechaNacimiento = fecha.ToString("yyyy-MM-dd");
                }
            }

            if (!string.IsNullOrEmpty(oferente.FechaPostulacion))
            {
                if (DateTime.TryParse(oferente.FechaPostulacion, out DateTime fecha))
                {
                    oferente.FechaPostulacion = fecha.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        return oferente;
    }
}