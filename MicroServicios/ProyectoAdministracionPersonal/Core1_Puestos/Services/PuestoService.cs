using Core1_Puestos.Entities;
using Core1_Puestos.Repository;

namespace Core1_Puestos.Services;

public class PuestoService : IPuestoService
{
    private readonly IPuestoRepository
        _puestoRepository;

    public PuestoService(
        IPuestoRepository puestoRepository)
    {
        _puestoRepository =
            puestoRepository;
    }

    public async Task<ResultadoPaginado<PuestoDTO>>
        ObtenerPuestosActivosAsync(
            int pagina,
            int tamanoPagina)
    {
        if (pagina <= 0)
        {
            throw new ArgumentException(
                "La página debe ser mayor que cero.");
        }

        if (tamanoPagina <= 0 ||
            tamanoPagina > 100)
        {
            throw new ArgumentException(
                "El tamaño de página debe estar entre 1 y 100.");
        }

        IEnumerable<PuestoDTO> puestos =
            await _puestoRepository
                .ObtenerPuestosActivosAsync(
                    pagina,
                    tamanoPagina);

        int totalRegistros =
            await _puestoRepository
                .ContarPuestosActivosAsync();

        int totalPaginas =
            (int)Math.Ceiling(
                totalRegistros /
                (double)tamanoPagina);

        return new ResultadoPaginado<PuestoDTO>
        {
            Datos = puestos,
            PaginaActual = pagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas
        };
    }
}