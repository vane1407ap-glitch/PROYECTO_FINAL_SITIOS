import { useNavigate } from "react-router-dom";

function Header() {
  const navigate = useNavigate();

  const nombreUsuario =
    localStorage.getItem("nombreUsuario") ||
    "Usuario";

  function cerrarSesion() {
    localStorage.removeItem("token");
    localStorage.removeItem("nombreUsuario");
    localStorage.removeItem("usuario");

    navigate("/");
  }

  return (
    <header className="topbar">
      <div>
        <span className="topbar-title">
          Administración de Personal
        </span>

        <span className="topbar-subtitle">
          Sistema de Gestión de Recursos Humanos
        </span>
      </div>

      <div className="topbar-user">
        <div className="user-info">
          <span className="user-label">
            Sesión iniciada como
          </span>

          <strong>{nombreUsuario}</strong>
        </div>

        <button
          type="button"
          className="btn-logout"
          onClick={cerrarSesion}
        >
          Cerrar sesión
        </button>
      </div>
    </header>
  );
}

export default Header;