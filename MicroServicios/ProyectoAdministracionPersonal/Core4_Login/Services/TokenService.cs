using Core4_Login.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core4_Login.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration
        _configuration;

    public TokenService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarToken(
        Usuario usuario)
    {
        string secretKey =
            _configuration[
                "JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException(
                "No se configuró la clave JWT.");

        string issuer =
            _configuration[
                "JwtSettings:Issuer"]
            ?? "Core4_Login";

        string audience =
            _configuration[
                "JwtSettings:Audience"]
            ?? "Microservicios";

        int minutos =
            _configuration.GetValue<int>(
                "JwtSettings:ExpirationMinutes",
                5);

        List<Claim> claims =
        [
            new Claim(
                JwtRegisteredClaimNames.Sub,
                usuario.IdUsuario.ToString()),

            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.IdUsuario.ToString()),

            new Claim(
                ClaimTypes.Name,
                usuario.NombreUsuario),

            new Claim(
                "nombre_completo",
                usuario.NombreCompleto),

            new Claim(
                JwtRegisteredClaimNames.Email,
                usuario.Correo),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        ];

        if (usuario.IdRol.HasValue)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    usuario.IdRol.Value.ToString()));

            claims.Add(
                new Claim(
                    "id_rol",
                    usuario.IdRol.Value.ToString()));
        }

        SymmetricSecurityKey securityKey =
            new(
                Encoding.UTF8.GetBytes(
                    secretKey));

        SigningCredentials credentials =
            new(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwt =
            new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires:
                    DateTime.UtcNow.AddMinutes(
                        minutos),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(jwt);
    }

    public ClaimsPrincipal? ValidarToken(
        string token)
    {
        try
        {
            string secretKey =
                _configuration[
                    "JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException(
                    "No se configuró la clave JWT.");

            string issuer =
                _configuration[
                    "JwtSettings:Issuer"]
                ?? "Core4_Login";

            string audience =
                _configuration[
                    "JwtSettings:Audience"]
                ?? "Microservicios";

            TokenValidationParameters parametros =
                new()
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                secretKey)),

                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };

            JwtSecurityTokenHandler handler =
                new();

            ClaimsPrincipal principal =
                handler.ValidateToken(
                    token,
                    parametros,
                    out SecurityToken tokenValidado);

            if (tokenValidado
                is not JwtSecurityToken jwt)
            {
                return null;
            }

            if (!jwt.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison
                        .OrdinalIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}