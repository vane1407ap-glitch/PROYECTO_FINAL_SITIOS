// src/pages/Login.jsx

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { gateway } from "../services/gateway";
import "../styles/login.css";

function Login() {
    const [usuario, setUsuario] = useState("");
    const [contrasena, setContrasena] = useState("");
    const [cargando, setCargando] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    async function iniciarSesion(e) {
        e.preventDefault();
        setError("");
        setCargando(true);

        try {
            if (!usuario || !contrasena) {
                throw new Error("Por favor, ingrese usuario y contraseña");
            }

            await gateway.login(usuario, contrasena);
            console.log("Login exitoso, redirigiendo...");
            navigate("/bienvenida");
        } catch (err) {
            setError(err.message || "No fue posible iniciar sesión.");
            console.error("Error de login:", err);
        } finally {
            setCargando(false);
        }
    }

    return (
        <div className="login-page">
            <div className="login-card">
                <div className="login-header">
                    <h1>Administración de Personal</h1>
                    <p>Inicio de sesión</p>
                </div>

                <form className="login-form" onSubmit={iniciarSesion}>
                    <div className="form-group">
                        <label>Usuario</label>
                        <input
                            type="text"
                            value={usuario}
                            onChange={(e) => setUsuario(e.target.value)}
                            placeholder="Ingrese su usuario"
                            required
                            disabled={cargando}
                            autoComplete="username"
                        />
                    </div>

                    <div className="form-group">
                        <label>Contraseña</label>
                        <input
                            type="password"
                            value={contrasena}
                            onChange={(e) => setContrasena(e.target.value)}
                            placeholder="Ingrese su contraseña"
                            required
                            disabled={cargando}
                            autoComplete="current-password"
                        />
                    </div>

                    <button
                        className="login-button"
                        type="submit"
                        disabled={cargando}
                    >
                        {cargando ? "Iniciando sesión..." : "Iniciar sesión"}
                    </button>

                    {error && <p className="mensaje-error">{error}</p>}
                </form>
            </div>
        </div>
    );
}

export default Login;