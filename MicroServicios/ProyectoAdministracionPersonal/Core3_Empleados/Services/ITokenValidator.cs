namespace Core3_Empleados.Services;

public interface ITokenValidator
{
    Task<bool> ValidarTokenAsync(
        string token);
}