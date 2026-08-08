namespace Core7_ListadoOferentes.Services;

public interface ITokenValidator
{
    Task<bool> ValidarTokenAsync(string token);
}