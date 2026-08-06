import { NavLink } from "react-router-dom";

function Sidebar() {
  return (
    <aside className="sidebar">
      <div className="sidebar-brand">
        <div className="brand-icon">AP</div>

        <div>
          <span className="sidebar-title">
            AdminPersonal
          </span>

          <span className="sidebar-subtitle">
            Recursos Humanos
          </span>
        </div>
      </div>

        <nav className="sidebar-menu">

            <NavLink
                to="/bienvenida"
                className={({ isActive }) =>
                    isActive
                        ? "sidebar-link active"
                        : "sidebar-link"
                }
            >
                Inicio
            </NavLink>

            <NavLink
                to="/puestos"
                className={({ isActive }) =>
                    isActive
                        ? "sidebar-link active"
                        : "sidebar-link"
                }
            >
                Puestos activos
            </NavLink>

        </nav>
    </aside>
  );
}

export default Sidebar;