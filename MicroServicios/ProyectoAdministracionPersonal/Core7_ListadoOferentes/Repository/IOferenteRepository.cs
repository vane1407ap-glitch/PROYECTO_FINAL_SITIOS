using Core7_ListadoOferentes.Entities;

namespace Core7_ListadoOferentes.Repository;

public interface IOferenteRepository
{
    Task<int> ObtenerIdPuestoPorCodigoAsync(string codigoPuesto);
    Task<IEnumerable<OferenteResumenDTO>> ObtenerOferentesPorIdPuestoAsync(int idPuesto);
    Task<OferenteDetalleDTO?> ObtenerDetalleOferenteAsync(int idPostulacion);
}