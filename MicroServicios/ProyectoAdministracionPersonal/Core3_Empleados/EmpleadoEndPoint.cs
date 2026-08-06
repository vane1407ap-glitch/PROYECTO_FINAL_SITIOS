using Core3_Empleados.Entities;
using Core3_Empleados.Services;

namespace Core3_Empleados;

public static class EmpleadoEndPoint
{
    public static void MapEmpleadoEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/Empleados")
            .WithTags("Empleados");

        group.MapPost("/", async (
            HttpContext context,
            CrearEmpleadoRequest request,
            IEmpleadoService service,
            ITokenValidator tokenValidator) =>
        {
            try
            {
                string? token =
                    ObtenerToken(context);

                if (string.IsNullOrWhiteSpace(
                    token))
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

                EmpleadoDTO empleado =
                    await service
                        .CrearEmpleadoAsync(
                            request);

                return Results.Created(
                    $"/api/Empleados/{empleado.IdEmpleado}",
                    new
                    {
                        codigo = 201,
                        mensaje =
                            "Empleado creado correctamente.",
                        data = empleado
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
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new
                {
                    codigo = 409,
                    mensaje = ex.Message,
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al crear empleado: {ex.Message}");

                return Results.Json(
                    new
                    {
                        codigo = 500,
                        mensaje =
                            "No se pudo crear el empleado.",
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