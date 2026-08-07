using Core8_Oferentes.Entities;

namespace Core8_Oferentes.Services;

public interface IOferenteCORE8Service
{
    Task<OferenteCORE8DTO> ObtenerOferenteAsync(string codigo);
}