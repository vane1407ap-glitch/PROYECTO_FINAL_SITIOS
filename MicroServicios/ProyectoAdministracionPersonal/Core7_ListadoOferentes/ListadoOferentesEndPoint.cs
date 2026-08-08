using Core7_ListadoOferentes.Entities;
using Core7_ListadoOferentes.Services;

namespace Core7_ListadoOferentes;

public static class ListadoOferentesEndPoint
{
    public static void MapListadoOferentesEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/Oferentes")
            .WithTags("Oferentes");

        // ==========================================
        // GET: OBTENER OFERENTES POR PUESTO
        // ==========================================

        group.MapGet("/por-puesto/{codigoPuesto}", async (
            HttpContext context,
            string codigoPuesto,
            IOferenteService service,
            ITokenValidator tokenValidator) =>
        {
            try
            {
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

                if (string.IsNullOrWhiteSpace(codigoPuesto))
                {
                    return Results.BadRequest(new
                    {
                        codigo = 400,
                        mensaje = "El código del puesto es requerido.",
                        data = (object?)null
                    });
                }

                var oferentes =
                    await service.ObtenerOferentesPorPuestoAsync(codigoPuesto);

                return Results.Ok(new
                {
                    codigo = 200,
                    mensaje = oferentes.Any()
                        ? "Oferentes obtenidos correctamente."
                        : "No hay oferentes para este puesto.",
                    data = new
                    {
                        codigoPuesto,
                        total = oferentes.Count(),
                        oferentes
                    }
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
            catch (Exception ex) when (ex.Message.Contains("Puesto no encontrado"))
            {
                return Results.NotFound(new
                {
                    codigo = 404,
                    mensaje = "El puesto especificado no existe.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener oferentes: {ex.Message}");
                return Results.Json(
                    new
                    {
                        codigo = 500,
                        mensaje = "No se pudieron obtener los oferentes.",
                        data = (object?)null
                    },
                    statusCode: 500);
            }
        });

        // ==========================================
        // GET: OBTENER DETALLE DE OFERENTE
        // ==========================================

        group.MapGet("/detalle/{idPostulacion}", async (
            HttpContext context,
            string idPostulacion,
            IOferenteService service,
            ITokenValidator tokenValidator) =>
        {
            try
            {
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

                if (string.IsNullOrWhiteSpace(idPostulacion))
                {
                    return Results.BadRequest(new
                    {
                        codigo = 400,
                        mensaje = "El ID de postulación es requerido.",
                        data = (object?)null
                    });
                }

                var oferente =
                    await service.ObtenerDetalleOferenteAsync(idPostulacion);

                return Results.Ok(new
                {
                    codigo = 200,
                    mensaje = "Detalle del oferente obtenido correctamente.",
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
                Console.WriteLine($"Error al obtener detalle del oferente: {ex.Message}");
                return Results.Json(
                    new
                    {
                        codigo = 500,
                        mensaje = "No se pudo obtener el detalle del oferente.",
                        data = (object?)null
                    },
                    statusCode: 500);
            }
        });

        // ==========================================
        // OPTIONS: MANEJO DE CORS (Usando MapMethods)
        // ==========================================

        group.MapMethods("/por-puesto/{codigoPuesto}", new[] { "OPTIONS" }, () =>
        {
            return Results.Ok();
        });

        group.MapMethods("/detalle/{idPostulacion}", new[] { "OPTIONS" }, () =>
        {
            return Results.Ok();
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