using Core4_Login.Entities;
using Core4_Login.Repository;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core4_Login.Services;

public class LoginService : ILoginService
{
    private readonly UsuarioRepository
        _usuarioRepository;

    private readonly ITokenService
        _tokenService;

    private readonly IConfiguration
        _configuration;

    public LoginService(
        UsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _usuarioRepository =
            usuarioRepository;

        _tokenService =
            tokenService;

        _configuration =
            configuration;
    }

    public async Task<LoginResponse>
        IniciarSesionAsync(
            LoginRequest request)
    {
        if (request is null ||
            string.IsNullOrWhiteSpace(
                request.Usuario) ||
            string.IsNullOrWhiteSpace(
                request.Contrasena))
        {
            return new LoginResponse
            {
                Exito = false,
                Mensaje =
                    "Debe ingresar el usuario y la contraseña."
            };
        }

        Usuario? usuario =
            await _usuarioRepository
                .ObtenerPorNombreUsuarioAsync(
                    request.Usuario.Trim());

        if (usuario is null)
        {
            return new LoginResponse
            {
                Exito = false,
                Mensaje =
                    "Usuario o contraseña incorrectos."
            };
        }

        if (!string.Equals(
                usuario.Estado.Trim(),
                "activo",
                StringComparison.OrdinalIgnoreCase))
        {
            return new LoginResponse
            {
                Exito = false,
                Mensaje =
                    "El usuario no se encuentra activo."
            };
        }

        if (usuario.Bloqueado)
        {
            return new LoginResponse
            {
                Exito = false,
                Mensaje =
                    "El usuario se encuentra bloqueado."
            };
        }

        string hashIngresado =
            GenerarSha256(
                request.Contrasena);

        bool contrasenaCorrecta =
            string.Equals(
                hashIngresado,
                usuario.Contrasena.Trim(),
                StringComparison.OrdinalIgnoreCase);

        if (!contrasenaCorrecta)
        {
            await _usuarioRepository
                .RegistrarIntentoFallidoAsync(
                    usuario.IdUsuario);

            int nuevosIntentos =
                usuario.IntentosFallidos + 1;

            if (nuevosIntentos >= 3)
            {
                return new LoginResponse
                {
                    Exito = false,
                    Mensaje =
                        "El usuario fue bloqueado después de 3 intentos fallidos."
                };
            }

            int restantes =
                3 - nuevosIntentos;

            return new LoginResponse
            {
                Exito = false,
                Mensaje =
                    $"Usuario o contraseña incorrectos. Intentos restantes: {restantes}."
            };
        }

        await _usuarioRepository
            .ReiniciarIntentosAsync(
                usuario.IdUsuario);

        string token =
            _tokenService.GenerarToken(
                usuario);

        int expiracion =
            _configuration.GetValue<int>(
                "JwtSettings:ExpirationMinutes",
                5);

        return new LoginResponse
        {
            Exito = true,
            Mensaje =
                "Inicio de sesión exitoso.",

            Token = token,
            IdUsuario = usuario.IdUsuario,
            NombreUsuario =
                usuario.NombreUsuario,
            NombreCompleto =
                usuario.NombreCompleto,
            Correo = usuario.Correo,
            IdRol = usuario.IdRol,
            ExpiracionMinutos = expiracion
        };
    }

    public async Task<ValidacionTokenResponse>
        ValidarTokenAsync(string token)
    {
        ClaimsPrincipal? principal =
            _tokenService.ValidarToken(token);

        if (principal is null)
        {
            return new ValidacionTokenResponse
            {
                Valido = false,
                Mensaje =
                    "El token es inválido o ya expiró."
            };
        }

        string? idTexto =
            principal.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!int.TryParse(
                idTexto,
                out int idUsuario))
        {
            return new ValidacionTokenResponse
            {
                Valido = false,
                Mensaje =
                    "El token no contiene un usuario válido."
            };
        }

        Usuario? usuario =
            await _usuarioRepository
                .ObtenerPorIdAsync(idUsuario);

        if (usuario is null)
        {
            return new ValidacionTokenResponse
            {
                Valido = false,
                Mensaje =
                    "El usuario del token no existe."
            };
        }

        if (usuario.Bloqueado)
        {
            return new ValidacionTokenResponse
            {
                Valido = false,
                Mensaje =
                    "El usuario se encuentra bloqueado."
            };
        }

        if (!string.Equals(
                usuario.Estado.Trim(),
                "activo",
                StringComparison.OrdinalIgnoreCase))
        {
            return new ValidacionTokenResponse
            {
                Valido = false,
                Mensaje =
                    "El usuario se encuentra inactivo."
            };
        }

        return new ValidacionTokenResponse
        {
            Valido = true,
            Mensaje = "Token válido.",
            IdUsuario = usuario.IdUsuario,
            NombreUsuario =
                usuario.NombreUsuario,
            NombreCompleto =
                usuario.NombreCompleto,
            IdRol = usuario.IdRol
        };
    }

    private static string GenerarSha256(
        string contrasena)
    {
        byte[] bytes =
            Encoding.UTF8.GetBytes(
                contrasena);

        byte[] hash =
            SHA256.HashData(bytes);

        return Convert
            .ToHexString(hash)
            .ToLowerInvariant();
    }
}