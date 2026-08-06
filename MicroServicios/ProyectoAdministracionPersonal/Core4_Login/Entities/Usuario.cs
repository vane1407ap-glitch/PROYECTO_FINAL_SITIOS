namespace Core4_Login.Entities;

public class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;

    public int IntentosFallidos { get; set; }

    public bool Bloqueado { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public int? IdRol { get; set; }
}

public class LoginRequest
{
    public string Usuario { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Exito { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public string? Token { get; set; }

    public int? IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Correo { get; set; }

    public int? IdRol { get; set; }

    public int ExpiracionMinutos { get; set; }
}

public class ValidacionTokenResponse
{
    public bool Valido { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public int? IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? NombreCompleto { get; set; }

    public int? IdRol { get; set; }
}