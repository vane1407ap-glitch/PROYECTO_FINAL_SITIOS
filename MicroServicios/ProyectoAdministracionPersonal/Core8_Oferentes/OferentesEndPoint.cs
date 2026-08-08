using Core8_Oferentes.Entities;
using Core8_Oferentes.Services;

namespace Core8_Oferentes;

public static class OferentesEndPoint
{
    public static void MapOferentesEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/Oferentes")
            .WithTags("Oferentes");

        // ==========================================
        // GET: OBTENER OFERENTE POR CÓDIGO
        // ==========================================

        group.MapGet("/{codigo}", async (
            HttpContext context,
            string codigo,
            IOferenteCORE8Service service,
            ITokenValidator tokenValidator) =>
        {
            try
            {
                // 1. Validar token
                string? token =
                    ObtenerToken(context);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return Results.Json(
                        new
                        {
                            codigo = 401,
                            mensaje = "No se recibió el token.",
                            data = (object?)null
                        },
                        statusCode: 401);
                }

                bool tokenValido =
                    await tokenValidator.ValidarTokenAsync(token);

                if (!tokenValido)
                {
                    return Results.Json(
                        new
                        {
                            codigo = 401,
                            mensaje = "El token es inválido o ya expiró.",
                            data = (object?)null
                        },
                        statusCode: 401);
                }

                // 2. Validar parámetros
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    return Results.BadRequest(new
                    {
                        codigo = 400,
                        mensaje = "El código del oferente es requerido.",
                        data = (object?)null
                    });
                }

                // 3. Obtener oferente
                var oferente =
                    await service.ObtenerOferenteAsync(codigo);

                // 4. Respuesta exitosa
                return Results.Ok(new
                {
                    codigo = 200,
                    mensaje = "Oferente obtenido correctamente.",
                    data = oferente
                });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    codigo = 400,
                    mensaje = ex.Message,
                    data = (object?)null
                });
            }
            catch (Exception ex) when (ex.Message.Contains("Oferente no encontrado"))
            {
                return Results.NotFound(new
                {
                    codigo = 404,
                    mensaje = "El oferente especificado no existe.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener oferente: {ex.Message}");
                return Results.Json(
                    new
                    {
                        codigo = 500,
                        mensaje = "No se pudo obtener el oferente.",
                        data = (object?)null
                    },
                    statusCode: 500);
            }
        });
    }

    private static string? ObtenerToken(
        HttpContext context)
    {
        string authorization =
            context.Request.Headers
                .Authorization
                .ToString();

        if (string.IsNullOrWhiteSpace(authorization))
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