using Core4_Login.Entities;
using Core4_Login.Services;

namespace Core4_Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/Login")
            .WithTags("Login");

        // ==========================================
        // POST: INICIAR SESIÓN
        // ==========================================

        group.MapPost("/", async (
            LoginRequest request,
            ILoginService service) =>
        {
            try
            {
                LoginResponse resultado =
                    await service.IniciarSesionAsync(
                        request);

                if (!resultado.Exito)
                {
                    return Results.BadRequest(new
                    {
                        codigo = 400,
                        mensaje = resultado.Mensaje,
                        data = resultado
                    });
                }

                return Results.Ok(new
                {
                    codigo = 200,
                    mensaje = resultado.Mensaje,
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error en login: {ex.Message}");

                return Results.Problem(
                    statusCode: 500,
                    title: "Error interno",
                    detail:
                        "No se pudo procesar el inicio de sesión.");
            }
        });

        // ==========================================
        // GET: VALIDAR TOKEN
        // ==========================================

        group.MapGet("/validar", async (
            HttpContext context,
            ILoginService service) =>
        {
            string? token =
                ObtenerToken(context);

            if (string.IsNullOrWhiteSpace(token))
            {
                return Results.Json(
                    new
                    {
                        codigo = 401,
                        mensaje =
                            "No se recibió el token.",
                        valido = false
                    },
                    statusCode: 401);
            }

            ValidacionTokenResponse resultado =
                await service.ValidarTokenAsync(
                    token);

            if (!resultado.Valido)
            {
                return Results.Json(
                    new
                    {
                        codigo = 401,
                        mensaje = resultado.Mensaje,
                        valido = false
                    },
                    statusCode: 401);
            }

            return Results.Ok(new
            {
                codigo = 200,
                mensaje = resultado.Mensaje,
                valido = true,
                data = resultado
            });
        });
    }

    private static string? ObtenerToken(
        HttpContext context)
    {
        string authorization =
            context.Request.Headers
                .Authorization
                .ToString();

        if (string.IsNullOrWhiteSpace(
                authorization))
        {
            return null;
        }

        const string prefijo = "Bearer ";

        if (!authorization.StartsWith(
                prefijo,
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authorization[
            prefijo.Length..].Trim();
    }
}