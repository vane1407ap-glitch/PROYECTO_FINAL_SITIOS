import { authService } from './authService';
import { API_GATEWAY } from './gateway';

export const empleadoService = {

  async crearEmpleado(empleado) {
    try {
      const token = authService.getToken();

      if (!token) {
        throw new Error(
          'No hay sesión activa.'
        );
      }

      const url =
        `${API_GATEWAY}/api/Empleados`;

      console.log(
        'Creando empleado en:',
        url
      );

      console.log(
        'Datos enviados:',
        empleado
      );

      const response = await fetch(
        url,
        {
          method: 'POST',

          headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            ...authService.getAuthHeaders()
          },

          body: JSON.stringify(
            empleado
          )
        }
      );

      const texto =
        await response.text();

      let data = {};

      if (texto) {
        try {
          data = JSON.parse(texto);
        } catch {
          data = {};
        }
      }

      if (response.status === 401) {
        authService.removeToken();

        throw new Error(
          'Sesión expirada. Por favor, inicie sesión nuevamente.'
        );
      }

      if (!response.ok) {
        throw new Error(
          data.mensaje ||
          data.message ||
          `No se pudo crear el empleado. Código: ${response.status}`
        );
      }

      return {
        exito: true,

        mensaje:
          data.mensaje ||
          'Empleado creado correctamente.',

        data:
          data.data || data
      };

    } catch (error) {
      console.error(
        'Error al crear empleado:',
        error
      );

      return {
        exito: false,

        mensaje:
          error.message ||
          'Error al crear el empleado.'
      };
    }
  }
};