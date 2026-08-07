// src/pages/Oferentes.jsx

import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { oferenteService } from '../services/oferenteService';
import '../styles/oferentes.css';

function Oferentes() {
  const [oferentes, setOferentes] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);
  const [codigoPuesto, setCodigoPuesto] = useState('');
  const [nombrePuesto, setNombrePuesto] = useState('');
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    // Obtener parámetros de la URL
    const params = new URLSearchParams(location.search);
    const codigo = params.get('codigo_puesto') || params.get('id') || '';
    const nombre = params.get('nombre_puesto') || params.get('nombre') || '';

    if (!codigo) {
      setError('No se recibió un código de puesto válido.');
      setCargando(false);
      return;
    }

    setCodigoPuesto(codigo);
    setNombrePuesto(nombre);
    cargarOferentes(codigo);
  }, [location.search]);

  const cargarOferentes = async (codigo) => {
    setCargando(true);
    setError(null);

    try {
      const resultado = await oferenteService.obtenerOferentesPorPuesto(codigo);
      
      if (resultado.exito) {
        setOferentes(resultado.oferentes || []);
      } else {
        setError(resultado.mensaje || 'No se pudieron cargar los oferentes');
        setOferentes([]);
      }
    } catch (err) {
      setError(err.message || 'Error al cargar los oferentes');
      setOferentes([]);
    } finally {
      setCargando(false);
    }
  };

  const handleVolver = () => {
    navigate('/puestos');
  };

  const handleVerDetalle = (codigoOferente) => {
    // Navegar a la página de detalle con los parámetros
    navigate(`/detalle-oferente?codigo=${encodeURIComponent(codigoOferente)}&codigo_puesto=${encodeURIComponent(codigoPuesto)}&nombre_puesto=${encodeURIComponent(nombrePuesto)}`);
  };

  const renderContenido = () => {
    if (cargando) {
      return (
        <tr>
          <td colSpan="2" className="loading-cell">
            <div className="spinner"></div>
            <span>Cargando oferentes...</span>
          </td>
        </tr>
      );
    }

    if (error) {
      return (
        <tr>
          <td colSpan="2" className="error-cell">
            <span className="error-icon"></span>
            {error}
          </td>
        </tr>
      );
    }

    if (oferentes.length === 0) {
      return (
        <tr>
          <td colSpan="2" className="empty-cell">
            <span className="empty-icon"></span>
            No existen oferentes para este puesto.
          </td>
        </tr>
      );
    }

    return oferentes.map((oferente, index) => (
      <tr key={index}>
        <td>
          <strong>{oferente.identificacion || 'N/A'}</strong>
        </td>
        <td>
          {oferente.codigoOferente ? (
            <button
              className="btn-oferente-link"
              onClick={() => handleVerDetalle(oferente.codigoOferente)}
            >
              {oferente.nombre || 'Ver oferente'}
              <span className="link-arrow">→</span>
            </button>
          ) : (
            <span>{oferente.nombre || 'Oferente sin código'}</span>
          )}
        </td>
      </tr>
    ));
  };

  return (
    <div className="oferentes-container">
      <section className="page-header">
        <div className="header-actions">
          <button onClick={handleVolver} className="btn-cancelar">
            ← Volver a puestos
          </button>
        </div>
        <div className="header-title">
          <h1>Oferentes</h1>
          <p>{nombrePuesto || 'Puesto seleccionado'}</p>
        </div>
        <p className="header-instruction">
          Seleccione el nombre de un oferente para consultar su información.
        </p>
      </section>

      <section className="table-card">
        <div className="table-card-header">
          <h2>Lista de oferentes</h2>
          <span>
            Código del puesto: <strong>{codigoPuesto}</strong>
          </span>
        </div>

        <div className="table-responsive">
          <table className="tabla">
            <thead>
              <tr>
                <th>Identificación</th>
                <th>Nombre del oferente</th>
              </tr>
            </thead>
            <tbody>
              {renderContenido()}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
}

export default Oferentes;