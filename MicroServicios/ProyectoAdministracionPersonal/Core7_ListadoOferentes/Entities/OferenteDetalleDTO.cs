namespace Core7_ListadoOferentes.Entities;

public class OferenteDetalleDTO
{
    public int IdPostulacion { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string FechaNacimiento { get; set; } = string.Empty;
    public string Curriculum { get; set; } = string.Empty;
    public string FechaPostulacion { get; set; } = string.Empty;
    public string NombrePuesto { get; set; } = string.Empty;
    public string CodigoPuesto { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public string EstadoPuesto { get; set; } = string.Empty;
}