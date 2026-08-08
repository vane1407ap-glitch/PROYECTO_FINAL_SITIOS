import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

import { oferenteService } from '../services/oferenteService';
import { empleadoService } from '../services/empleadoService';

import '../styles/detalle-oferente.css';

function DetalleOferente() {
  const [detalle, setDetalle] = useState(null);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);

  const [codigoPuesto, setCodigoPuesto] = useState('');
  const [nombrePuesto, setNombrePuesto] = useState('');
  const [idPuesto, setIdPuesto] = useState('');

  const [numeroEmpleado, setNumeroEmpleado] = useState('');
  const [fechaContratacion, setFechaContratacion] = useState('');

  const [guardando, setGuardando] = useState(false);
  const [mensaje, setMensaje] = useState('');

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const params = new URLSearchParams(location.search);

    const codigo = params.get('codigo');
    const codigoPuestoParam = params.get('codigo_puesto') || '';
    const nombrePuestoParam = params.get('nombre_puesto') || '';
    const idPuestoParam = params.get('id_puesto') || '';

    setCodigoPuesto(codigoPuestoParam);
    setNombrePuesto(nombrePuestoParam);
    setIdPuesto(idPuestoParam);

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
      const resultado =
        await oferenteService.obtenerDetalleOferente(codigo);

      if (resultado.exito) {
        setDetalle(resultado.detalle);
      } else {
        setError(
          resultado.mensaje ||
          'No se pudo obtener el detalle del oferente'
        );

        setDetalle(null);
      }
    } catch (err) {
      setError(
        err.message ||
        'Error al cargar el detalle del oferente'
      );

      setDetalle(null);
    } finally {
      setCargando(false);
    }
  };

  const handleCancelar = () => {
    navigate('/puestos');
  };

  const handleCrearEmpleado = async () => {
    setMensaje('');

    if (!numeroEmpleado.trim()) {
      setMensaje('Debe ingresar el número de empleado.');
      return;
    }

    if (!fechaContratacion) {
      setMensaje('Debe seleccionar la fecha de contratación.');
      return;
    }

    if (!idPuesto) {
      setMensaje('No se pudo identificar el puesto seleccionado.');
      return;
    }

    if (!detalle) {
      setMensaje('No se encontró la información del oferente.');
      return;
    }

    const idPuestoNumero = parseInt(idPuesto, 10);

    if (Number.isNaN(idPuestoNumero)) {
      setMensaje('El identificador del puesto no es válido.');
      return;
    }

    try {
      setGuardando(true);

      const empleado = {
        numeroEmpleado: numeroEmpleado.trim(),

        identificacion:
          detalle.identificacion || '',

        tipoIdentificacion:
          detalle.tipoIdentificacion || '',

        nombreCompleto:
          detalle.nombreCompleto ||
          detalle.nombre ||
          '',

        fechaNacimiento:
          detalle.fechaNacimiento
            ? new Date(detalle.fechaNacimiento)
                .toISOString()
                .split('T')[0]
            : '',

        correo:
          detalle.correo || '',

        telefono:
          detalle.telefono || '',

        idPuesto:
          idPuestoNumero,

        fechaContratacion:
          fechaContratacion,

        estado:
          'Activo'
      };

      console.log('Empleado enviado:', empleado);

      const resultado =
        await empleadoService.crearEmpleado(empleado);

      if (!resultado.exito) {
        setMensaje(
          resultado.mensaje ||
          'No se pudo crear el empleado.'
        );
        return;
      }

      alert(
        resultado.mensaje ||
        'Empleado creado correctamente.'
      );

      navigate('/puestos');

    } catch (err) {
      console.error(
        'Error creando empleado:',
        err
      );

      setMensaje(
        err.message ||
        'Ocurrió un error al crear el empleado.'
      );
    } finally {
      setGuardando(false);
    }
  };

  if (cargando) {
    return (
      <div className="detalle-container">
        <div className="loading-container">
          <div className="spinner"></div>

          <p>
            Cargando detalle del oferente...
          </p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="detalle-container">
        <div className="error-container">
          <p>{error}</p>

          <button
            type="button"
            onClick={handleCancelar}
          >
            Cancelar
          </button>
        </div>
      </div>
    );
  }

  if (!detalle) {
    return (
      <div className="detalle-container">
        <div className="error-container">
          <p>
            No se encontró el oferente.
          </p>

          <button
            type="button"
            onClick={handleCancelar}
          >
            Cancelar
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="detalle-container">

      <section className="page-header">
        <div>
          <h1>
            Detalle del Oferente
          </h1>

          <p>
            {detalle.nombreCompleto ||
              detalle.nombre ||
              'Oferente'}
          </p>
        </div>
      </section>

      <section className="detalle-card">

        <div className="detalle-grid">

          <div className="detalle-item">
            <label>
              Código de Oferente
            </label>

            <span>
              {detalle.codigoOferente || 'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Identificación
            </label>

            <span>
              {detalle.identificacion || 'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Tipo de Identificación
            </label>

            <span>
              {detalle.tipoIdentificacion || 'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Nombre Completo
            </label>

            <span>
              {detalle.nombreCompleto ||
                detalle.nombre ||
                'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Fecha de Nacimiento
            </label>

            <span>
              {detalle.fechaNacimiento || 'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Correo Electrónico
            </label>

            <span>
              {detalle.correo || 'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Teléfono
            </label>

            <span>
              {detalle.telefono || 'N/A'}
            </span>
          </div>

          <div className="detalle-item">
            <label>
              Puesto
            </label>

            <span>
              {nombrePuesto ||
                detalle.nombrePuesto ||
                'N/A'}
            </span>
          </div>

        </div>

        <div className="contratacion-section">

          <h3>
            Datos de contratación
          </h3>

          <div className="detalle-grid">

            <div className="detalle-item">
              <label>
                Número de empleado
              </label>

              <input
                type="text"
                value={numeroEmpleado}
                onChange={(e) =>
                  setNumeroEmpleado(
                    e.target.value
                  )
                }
                placeholder="Ingrese el número de empleado"
                disabled={guardando}
              />
            </div>

            <div className="detalle-item">
              <label>
                Fecha de contratación
              </label>

              <input
                type="date"
                value={fechaContratacion}
                onChange={(e) =>
                  setFechaContratacion(
                    e.target.value
                  )
                }
                disabled={guardando}
              />
            </div>

          </div>

          {mensaje && (
            <div className="mensaje-error">
              {mensaje}
            </div>
          )}

          <div className="detalle-actions">

            <button
              type="button"
              className="btn-cancelar"
              onClick={handleCancelar}
              disabled={guardando}
            >
              Cancelar
            </button>

            <button
              type="button"
              className="btn-primary"
              onClick={handleCrearEmpleado}
              disabled={guardando}
            >
              {guardando
                ? 'Creando empleado...'
                : 'Crear empleado'}
            </button>

          </div>

        </div>

      </section>

    </div>
  );
}

export default DetalleOferente;