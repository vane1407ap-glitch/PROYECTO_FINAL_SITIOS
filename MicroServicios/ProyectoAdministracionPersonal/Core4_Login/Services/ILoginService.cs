using Core4_Login.Entities;

namespace Core4_Login.Services;

public interface ILoginService
{
    Task<LoginResponse> IniciarSesionAsync(
        LoginRequest request);

    Task<ValidacionTokenResponse>
        ValidarTokenAsync(string token);
}