using Core1_Puestos.Entities;

namespace Core1_Puestos.Repository;

public interface IPuestoRepository
{
    Task<IEnumerable<PuestoDTO>>
        ObtenerPuestosActivosAsync(
            int pagina,
            int tamanoPagina);

    Task<int>
        ContarPuestosActivosAsync();
}