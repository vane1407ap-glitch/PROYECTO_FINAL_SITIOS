using Core7_ListadoOferentes.Entities;
using Core7_ListadoOferentes.Repository;

namespace Core7_ListadoOferentes.Services;

public class OferenteService : IOferenteService
{
    private readonly IOferenteRepository _oferenteRepository;

    public OferenteService(IOferenteRepository oferenteRepository)
    {
        _oferenteRepository = oferenteRepository;
    }

    public async Task<IEnumerable<OferenteResumenDTO>> ObtenerOferentesPorPuestoAsync(string codigoPuesto)
    {
        if (string.IsNullOrWhiteSpace(codigoPuesto))
        {
            throw new ArgumentException("El código del puesto es requerido");
        }

        try
        {
            int idPuesto = await _oferenteRepository.ObtenerIdPuestoPorCodigoAsync(codigoPuesto);
            return await _oferenteRepository.ObtenerOferentesPorIdPuestoAsync(idPuesto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en service ObtenerOferentesPorPuesto: {ex.Message}");
            throw;
        }
    }

    public async Task<OferenteDetalleDTO> ObtenerDetalleOferenteAsync(string idPostulacion)
    {
        if (string.IsNullOrWhiteSpace(idPostulacion))
        {
            throw new ArgumentException("El ID de postulación es requerido");
        }

        try
        {
            if (!int.TryParse(idPostulacion, out int id) || id <= 0)
            {
                throw new ArgumentException("El ID de postulación debe ser un número válido");
            }

            var oferente = await _oferenteRepository.ObtenerDetalleOferenteAsync(id);

            if (oferente == null)
            {
                throw new Exception("Oferente no encontrado");
            }

            return oferente;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en service ObtenerDetalleOferente: {ex.Message}");
            throw;
        }
    }
}