using Core1_Puestos.Entities;

namespace Core1_Puestos.Services;

public interface IPuestoService
{
    Task<ResultadoPaginado<PuestoDTO>>
        ObtenerPuestosActivosAsync(
            int pagina,
            int tamanoPagina);
}