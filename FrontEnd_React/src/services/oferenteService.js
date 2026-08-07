// src/services/oferenteService.js

import { authService } from './authService';

const CORE7_URL = import.meta.env.VITE_CORE7_URL || 'http://localhost:5210';
const CORE8_URL = import.meta.env.VITE_CORE8_URL || 'http://localhost:5246';

export const oferenteService = {
  // Core7 - Obtener oferentes por puesto
  async obtenerOferentesPorPuesto(codigoPuesto) {
    try {
      const token = authService.getToken();
      
      if (!token) {
        throw new Error('No hay sesión activa');
      }

      const url = `${CORE7_URL}/api/Oferentes/por-puesto/${encodeURIComponent(codigoPuesto)}`;
      
      console.log(' Buscando oferentes para:', codigoPuesto);
      console.log(' URL:', url);

      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      });

      if (response.status === 401) {
        authService.removeToken();
        throw new Error('Sesión expirada. Por favor, inicie sesión nuevamente.');
      }

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.mensaje || 'Error al obtener oferentes');
      }

      console.log(' Oferentes obtenidos:', data);

      // Normalizar la respuesta
      if (data.codigo === 200 && data.data) {
        return {
          exito: true,
          oferentes: data.data.oferentes || [],
          total: data.data.total || 0,
          codigoPuesto: data.data.codigoPuesto || codigoPuesto
        };
      }

      return {
        exito: false,
        mensaje: data.mensaje || 'No se pudieron obtener los oferentes',
        oferentes: []
      };
    } catch (error) {
      console.error('Error en obtenerOferentesPorPuesto:', error);
      return {
        exito: false,
        mensaje: error.message || 'Error al obtener los oferentes',
        oferentes: []
      };
    }
  },

  // Core8 - Obtener detalle de oferente por código
  async obtenerDetalleOferente(codigoOferente) {
    try {
      const token = authService.getToken();
      
      if (!token) {
        throw new Error('No hay sesión activa');
      }

      const url = `${CORE8_URL}/api/Oferentes/${encodeURIComponent(codigoOferente)}`;
      
      console.log(' Buscando detalle de:', codigoOferente);
      console.log(' URL:', url);

      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      });

      if (response.status === 401) {
        authService.removeToken();
        throw new Error('Sesión expirada. Por favor, inicie sesión nuevamente.');
      }

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.mensaje || 'Error al obtener detalle del oferente');
      }

      console.log('Detalle obtenido:', data);

      if (data.codigo === 200 && data.data) {
        return {
          exito: true,
          detalle: data.data
        };
      }

      return {
        exito: false,
        mensaje: data.mensaje || 'No se pudo obtener el detalle del oferente',
        detalle: null
      };
    } catch (error) {
      console.error(' Error en obtenerDetalleOferente:', error);
      return {
        exito: false,
        mensaje: error.message || 'Error al obtener el detalle del oferente',
        detalle: null
      };
    }
  }
};