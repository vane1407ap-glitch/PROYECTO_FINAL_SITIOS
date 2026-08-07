// src/pages/DetalleOferente.jsx

import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { oferenteService } from '../services/oferenteService';
import '../styles/detalle-oferente.css';

function DetalleOferente() {
  const [detalle, setDetalle] = useState(null);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);
  const [codigoPuesto, setCodigoPuesto] = useState('');
  const [nombrePuesto, setNombrePuesto] = useState('');
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const codigo = params.get('codigo');
    const codigoPuestoParam = params.get('codigo_puesto') || '';
    const nombrePuestoParam = params.get('nombre_puesto') || '';

    setCodigoPuesto(codigoPuestoParam);
    setNombrePuesto(nombrePuestoParam);

    if (!codigo) {
      setError('No se recibió un código de oferente válido.');
      setCargando(false);
      return;
    }

    cargarDetalle(codigo);
  }, [location.search]);

  const cargarDetalle = async (codigo) => {
    setCargando(true);
    setError(null);

    try {
      const resultado = await oferenteService.obtenerDetalleOferente(codigo);
      
      if (resultado.exito) {
        setDetalle(resultado.detalle);
      } else {
        setError(resultado.mensaje || 'No se pudo obtener el detalle del oferente');
        setDetalle(null);
      }
    } catch (err) {
      setError(err.message || 'Error al cargar el detalle');
      setDetalle(null);
    } finally {
      setCargando(false);
    }
  };

  const handleVolver = () => {
    navigate(`/oferentes?codigo_puesto=${encodeURIComponent(codigoPuesto)}&nombre_puesto=${encodeURIComponent(nombrePuesto)}`);
  };

  const handleVolverPuestos = () => {
    navigate('/puestos');
  };

  if (cargando) {
    return (
      <div className="detalle-container">
        <div className="loading-container">
          <div className="spinner-large"></div>
          <p>Cargando detalle del oferente...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="detalle-container">
        <div className="error-container">
          <span className="error-icon"></span>
          <h2>{error}</h2>
          <button onClick={handleVolver} className="btn-cancelar">
            ← Volver a oferentes
          </button>
        </div>
      </div>
    );
  }

  if (!detalle) {
    return (
      <div className="detalle-container">
        <div className="empty-container">
          <span className="empty-icon">📭</span>
          <h2>No se encontró el oferente</h2>
          <button onClick={handleVolver} className="btn-cancelar">
            ← Volver a oferentes
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="detalle-container">
      <section className="detalle-header">
        <div className="header-actions">
          <button onClick={handleVolver} className="btn-cancelar">
            ← Volver a oferentes
          </button>
          <button onClick={handleVolverPuestos} className="btn-cancelar-secondary">
            ← Volver a puestos
          </button>
        </div>
        <div className="header-title">
          <h1>Detalle del Oferente</h1>
          <p>{detalle.nombreCompleto || detalle.nombre || 'Oferente'}</p>
        </div>
      </section>

      <section className="detalle-card">
        <div className="detalle-grid">
          <div className="detalle-item">
            <label>Código de Oferente</label>
            <span>{detalle.codigoOferente || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Identificación</label>
            <span>{detalle.identificacion || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Tipo de Identificación</label>
            <span>{detalle.tipoIdentificacion || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Nombre Completo</label>
            <span>{detalle.nombreCompleto || detalle.nombre || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Fecha de Nacimiento</label>
            <span>{detalle.fechaNacimiento || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Correo Electrónico</label>
            <span>{detalle.correo || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Teléfono</label>
            <span>{detalle.telefono || 'N/A'}</span>
          </div>

          <div className="detalle-item">
            <label>Puesto</label>
            <span>{nombrePuesto || detalle.nombrePuesto || 'N/A'}</span>
          </div>
        </div>

        <div className="detalle-actions">
          <button onClick={handleVolver} className="btn-primary">
            ← Volver a la lista
          </button>
        </div>
      </section>
    </div>
  );
}

export default DetalleOferente;