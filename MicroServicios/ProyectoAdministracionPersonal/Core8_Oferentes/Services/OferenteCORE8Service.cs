using Core8_Oferentes.Entities;
using Core8_Oferentes.Repository;

namespace Core8_Oferentes.Services;

public class OferenteCORE8Service : IOferenteCORE8Service
{
    private readonly IOferenteCORE8Repository _oferenteRepository;

    public OferenteCORE8Service(IOferenteCORE8Repository oferenteRepository)
    {
        _oferenteRepository = oferenteRepository;
    }

    public async Task<OferenteCORE8DTO> ObtenerOferenteAsync(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
        {
            throw new ArgumentException("El código del oferente es requerido");
        }

        try
        {
            var oferente = await _oferenteRepository.ObtenerOferentePorCodigoAsync(codigo);

            if (oferente == null)
            {
                throw new Exception("Oferente no encontrado");
            }

            return oferente;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en service ObtenerOferente: {ex.Message}");
            throw;
        }
    }
}