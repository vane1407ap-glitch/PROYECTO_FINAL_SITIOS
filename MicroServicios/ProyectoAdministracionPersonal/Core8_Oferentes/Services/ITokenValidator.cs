namespace Core8_Oferentes.Services;

public interface ITokenValidator
{
    Task<bool> ValidarTokenAsync(string token);
}