namespace Core7_ListadoOferentes.Entities;

public class OferenteResumenDTO
{
    public int IdPostulacion { get; set; }
    public string CodigoOferente { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Curriculum { get; set; } = string.Empty;
    public string FechaPostulacion { get; set; } = string.Empty;
}