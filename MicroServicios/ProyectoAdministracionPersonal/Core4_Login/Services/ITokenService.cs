using Core4_Login.Entities;
using System.Security.Claims;

namespace Core4_Login.Services;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);

    ClaimsPrincipal? ValidarToken(
        string token);
}