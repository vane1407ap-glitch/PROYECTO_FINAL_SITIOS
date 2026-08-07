// src/components/Layout.jsx

import { Outlet, Link, useLocation, Navigate } from "react-router-dom";
import { authService } from "../services/authService";
import "../styles/layout.css";
import "../styles/sidebar.css";
import "../styles/header.css";

function Layout() {
    const location = useLocation();
    const nombreUsuario = authService.getNombreUsuario();
    const inicial = authService.getInicialUsuario();

    if (!authService.isAuthenticated()) {
        return <Navigate to="/" replace />;
    }

    const handleLogout = () => {
        authService.removeToken();
        window.location.href = "/";
    };

    const isActive = (path) => location.pathname === path ? "active" : "";

    return (
        <div className="app-container">
            {/* Sidebar - Usando tus estilos existentes */}
            <aside className="sidebar">
                <div className="sidebar-header">
                    <div className="sidebar-brand">
                        <div className="brand-icon">AP</div>
                        <span className="brand-name">AdminPersonal</span>
                    </div>
                </div>

                <nav className="sidebar-menu">
                    <Link to="/bienvenida" className={`sidebar-link ${isActive("/bienvenida")}`}>
                        <span className="sidebar-icon">⌂</span>
                        <span>Página de bienvenida</span>
                    </Link>

                    <Link to="/puestos" className={`sidebar-link ${isActive("/puestos")}`}>
                        <span className="sidebar-icon">▤</span>
                        <span>Puestos activos</span>
                    </Link>
                </nav>

                <div className="sidebar-footer">
                    <div className="sidebar-user">
                        <div className="sidebar-avatar">{inicial}</div>
                        <div className="sidebar-user-data">
                            <span>Sesión iniciada como</span>
                            <strong>{nombreUsuario}</strong>
                        </div>
                    </div>
                    <button onClick={handleLogout} className="btn-logout-sidebar">
                        Cerrar sesión
                    </button>
                </div>
            </aside>

            {/* Área de contenido */}
            <div className="content-area">
                {/* Header - Usando tus estilos existentes */}
                <header className="topbar">
                    <div className="topbar-left">
                        <h1 className="page-title">
                            {location.pathname === "/bienvenida" && "Bienvenida"}
                            {location.pathname === "/puestos" && "Puestos Activos"}
                        </h1>
                    </div>
                    <div className="topbar-right">
                        <div className="user-info-topbar">
                            <span className="user-label">Usuario</span>
                            <strong>{nombreUsuario}</strong>
                        </div>
                        <div className="user-avatar">{inicial}</div>
                    </div>
                </header>

                {/* Contenido principal */}
                <main className="main-content">
                    <Outlet />
                </main>
            </div>
        </div>
    );
}

export default Layout;