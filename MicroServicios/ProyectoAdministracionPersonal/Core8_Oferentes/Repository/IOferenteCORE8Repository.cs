using Core8_Oferentes.Entities;

namespace Core8_Oferentes.Repository;

public interface IOferenteCORE8Repository
{
    Task<OferenteCORE8DTO?> ObtenerOferentePorCodigoAsync(string codigo);
}