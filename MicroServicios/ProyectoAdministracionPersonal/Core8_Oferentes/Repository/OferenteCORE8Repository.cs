using Core8_Oferentes.Entities;
using Dapper;
using System.Data;

namespace Core8_Oferentes.Repository;

public class OferenteCORE8Repository : IOferenteCORE8Repository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public OferenteCORE8Repository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<OferenteCORE8DTO?> ObtenerOferentePorCodigoAsync(string codigo)
    {
        const string sql = """
            SELECT
                o.codigo_oferente AS CodigoOferente,
                o.identificacion AS Identificacion,
                o.tipo_identificacion AS TipoIdentificacion,
                o.nombre_completo AS NombreCompleto,
                o.fecha_nacimiento AS FechaNacimiento,
                o.correo AS Correo,
                o.telefono AS Telefono
            FROM oferentes o
            WHERE o.codigo_oferente = @Codigo
            """;

        using var connection = _connectionFactory.CreateConnection();

        var oferente = await connection
            .QueryFirstOrDefaultAsync<OferenteCORE8DTO>(sql, new { Codigo = codigo });

        if (oferente != null)
        {
            // Formatear fecha de nacimiento si existe
            if (!string.IsNullOrEmpty(oferente.FechaNacimiento))
            {
                if (DateTime.TryParse(oferente.FechaNacimiento, out DateTime fecha))
                {
                    oferente.FechaNacimiento = fecha.ToString("yyyy-MM-dd");
                }
            }
        }

        return oferente;
    }
}