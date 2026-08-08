namespace Core1_Puestos.Entities;

public class PuestoDTO
{
    public string CodigoPuesto { get; set; } = string.Empty;

    public string NombrePuesto { get; set; } = string.Empty;
}

public class ResultadoPaginado<T>
{
    public IEnumerable<T> Datos { get; set; } = [];

    public int PaginaActual { get; set; }

    public int TamanoPagina { get; set; }

    public int TotalRegistros { get; set; }

    public int TotalPaginas { get; set; }
}