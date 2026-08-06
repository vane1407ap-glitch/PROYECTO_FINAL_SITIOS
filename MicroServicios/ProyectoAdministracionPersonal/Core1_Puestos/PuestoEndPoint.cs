using Core1_Puestos.Entities;
using Core1_Puestos.Services;

namespace Core1_Puestos;

public static class PuestoEndPoint
{
    public static void MapPuestoEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/Puestos")
            .WithTags("Puestos");

        // ==========================================
        // GET: LISTAR PUESTOS ACTIVOS
        // ==========================================

        group.MapGet("/", async (
            HttpContext context,
            IPuestoService service,
            ITokenValidator tokenValidator,
            int pagina = 1,
            int tamanoPagina = 10) =>
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
                            mensaje =
                                "No se recibió el token.",
                            data = (object?)null
                        },
                        statusCode: 401);
                }

                bool tokenValido =
                    await tokenValidator
                        .ValidarTokenAsync(token);

                if (!tokenValido)
                {
                    return Results.Json(
                        new
                        {
                            codigo = 401,
                            mensaje =
                                "El token es inválido o ya expiró.",
                            data = (object?)null
                        },
                        statusCode: 401);
                }

                ResultadoPaginado<PuestoDTO>
                    resultado =
                        await service
                            .ObtenerPuestosActivosAsync(
                                pagina,
                                tamanoPagina);

                context.Response.Headers[
                    "X-Total-Count"] =
                    resultado.TotalRegistros
                        .ToString();

                context.Response.Headers[
                    "X-Page"] =
                    resultado.PaginaActual
                        .ToString();

                context.Response.Headers[
                    "X-Page-Size"] =
                    resultado.TamanoPagina
                        .ToString();

                context.Response.Headers[
                    "X-Total-Pages"] =
                    resultado.TotalPaginas
                        .ToString();

                return Results.Ok(new
                {
                    codigo = 200,
                    mensaje =
                        "Puestos activos obtenidos correctamente.",
                    data = resultado
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
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al obtener puestos: {ex.Message}");

                return Results.Json(
                    new
                    {
                        codigo = 500,
                        mensaje =
                            "No se pudieron obtener los puestos activos.",
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