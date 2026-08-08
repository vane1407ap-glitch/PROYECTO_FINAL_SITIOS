// src/pages/PuestosActivos.jsx

import React, { useState, useEffect, useCallback } from 'react';
import { Link } from 'react-router-dom';
import { gateway } from '../services/gateway';
import { authService } from '../services/authService';
import '../styles/puestos.css';

function PuestosActivos() {
    const [puestos, setPuestos] = useState([]);
    const [cargando, setCargando] = useState(true);
    const [error, setError] = useState(null);
    const [totalPuestos, setTotalPuestos] = useState(0);

    const cargarTodosLosPuestos = useCallback(async () => {
        setCargando(true);
        setError(null);

        try {
            if (!authService.isAuthenticated()) {
                setError('Sesion expirada. Por favor, inicie sesion nuevamente.');
                setCargando(false);
                return;
            }

            const resultado = await gateway.obtenerTodosLosPuestosActivos();
            
            console.log('Resultado final:', resultado);
            
            if (resultado.exito && resultado.puestos) {
                setPuestos(resultado.puestos);
                setTotalPuestos(resultado.puestos.length);
                console.log('Puestos cargados:', resultado.puestos.length);
            } else {
                setError(resultado.mensaje || 'No se pudieron cargar los puestos');
                setPuestos([]);
                setTotalPuestos(0);
            }
        } catch (err) {
            console.error('Error:', err);
            setError(err.message);
            setPuestos([]);
            setTotalPuestos(0);
        } finally {
            setCargando(false);
        }
    }, []);

    useEffect(() => {
        cargarTodosLosPuestos();
    }, [cargarTodosLosPuestos]);

    if (cargando) {
        return (
            <div className="puestos-container">
                <div className="loading-container">
                    <div className="spinner"></div>
                    <p>Cargando puestos...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="puestos-container">
                <div className="error-container">
                    <p>{error}</p>
                    <button onClick={() => window.location.reload()}>Reintentar</button>
                </div>
            </div>
        );
    }

    return (
        <div className="puestos-container">
            <section className="page-header">
                <div>
                    <h1>Puestos activos</h1>
                    <p>
                        Seleccione el nombre de un puesto para consultar
                        los oferentes asociados.
                    </p>
                </div>
                {totalPuestos > 0 && (
                    <div className="header-stats">
                        <span className="stat-badge">
                            Total: {totalPuestos} puestos activos
                        </span>
                    </div>
                )}
            </section>

            <section className="table-card">
                <div className="table-card-header">
                    <div>
                        <h2>Lista completa de puestos</h2>
                        <span className="subtitle-info">
                            {totalPuestos > 0 
                                ? 'Mostrando todos los ' + totalPuestos + ' puestos disponibles'
                                : 'Puestos disponibles actualmente'}
                        </span>
                    </div>
                </div>

                <div className="table-responsive">
                    <table className="tabla">
                        <thead>
                            <tr>
                                <th>Codigo</th>
                                <th>Nombre del puesto</th>
                            </tr>
                        </thead>
                        <tbody>
                            {puestos.length === 0 ? (
                                <tr>
                                    <td colSpan="2" className="empty-cell">
                                        No hay puestos activos disponibles.
                                    </td>
                                </tr>
                            ) : (
                                puestos.map((puesto, index) => (
                                    <tr key={index}>
                                        <td>
                                            <strong>{puesto.codigo || '-'}</strong>
                                        </td>
                                        <td>
                                            <Link 
                                                to={`/oferentes?codigo_puesto=${encodeURIComponent(puesto.codigo)}&nombre_puesto=${encodeURIComponent(puesto.nombre)}&id_puesto=${encodeURIComponent(puesto.idPuesto)}`}
                                                className="puesto-link"
                                            >
                                                {puesto.nombre || 'Sin nombre'}
                                                <span className="link-arrow">→</span>
                                            </Link>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>

                {totalPuestos > 0 && (
                    <div className="table-footer">
                        <span className="total-registros">
                            Total: {totalPuestos} puestos activos
                        </span>
                    </div>
                )}
            </section>
        </div>
    );
}

export default PuestosActivos;