using Core7_ListadoOferentes.Entities;

namespace Core7_ListadoOferentes.Services;

public interface IOferenteService
{
    Task<IEnumerable<OferenteResumenDTO>> ObtenerOferentesPorPuestoAsync(string codigoPuesto);
    Task<OferenteDetalleDTO> ObtenerDetalleOferenteAsync(string idPostulacion);
}