namespace Core1_Puestos.Services;

public interface ITokenValidator
{
    Task<bool> ValidarTokenAsync(
        string token);
}