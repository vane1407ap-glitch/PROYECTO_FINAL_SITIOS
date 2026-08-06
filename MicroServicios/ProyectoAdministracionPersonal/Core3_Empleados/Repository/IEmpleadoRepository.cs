using Core3_Empleados.Entities;

namespace Core3_Empleados.Repository;

public interface IEmpleadoRepository
{
    Task<bool> ExisteNumeroEmpleadoAsync(
        string numeroEmpleado);

    Task<bool> ExisteIdentificacionAsync(
        string identificacion);

    Task<bool> ExisteCorreoAsync(
        string correo);

    Task<bool> ExisteTelefonoAsync(
        string telefono);

    Task<bool> ExistePuestoAsync(
        int idPuesto);

    Task<EmpleadoDTO> CrearEmpleadoAsync(
        CrearEmpleadoRequest request,
        DateTime fechaNacimiento,
        DateTime fechaContratacion);
}