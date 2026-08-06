import { Link } from "react-router-dom";

function Bienvenida() {

    const nombreUsuario =
        localStorage.getItem("nombreUsuario")
        || "Usuario";

    return (

        <div className="welcome-page">

            <section className="welcome-hero">

                <div className="welcome-badge">
                    Panel administrativo
                </div>

                <h1>
                    Bienvenido, {nombreUsuario}
                </h1>

                <p>
                    Ha iniciado sesión correctamente.
                    Desde este panel puede consultar
                    los puestos disponibles en el sistema.
                </p>

            </section>

            <section className="option-grid">

                <Link
                    to="/puestos"
                    className="option-card"
                >

                    <div className="option-icon">
                        P
                    </div>

                    <div className="option-content">

                        <h2>
                            Puestos activos
                        </h2>

                        <p>
                            Consulte los puestos
                            disponibles, sus códigos,
                            nombres y salarios.
                        </p>

                        <span className="option-link">
                            Ir a puestos activos →
                        </span>

                    </div>

                </Link>

            </section>

        </div>

    );

}

export default Bienvenida;