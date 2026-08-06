namespace Core3_Empleados.Entities;

public class EmpleadoDTO
{
    public int IdEmpleado { get; set; }

    public string NumeroEmpleado { get; set; } = string.Empty;

    public string Identificacion { get; set; } = string.Empty;

    public string TipoIdentificacion { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string FechaNacimiento { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public int IdPuesto { get; set; }

    public string FechaContratacion { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
}