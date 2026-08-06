using Core3_Empleados.Entities;

namespace Core3_Empleados.Services;

public interface IEmpleadoService
{
    Task<EmpleadoDTO> CrearEmpleadoAsync(
        CrearEmpleadoRequest request);
}