import { authService } from './authService';

const API_URL =
  import.meta.env.VITE_API_URL ||
  'http://localhost:5184';

export const API_GATEWAY =
  import.meta.env.VITE_GATEWAY_URL ||
  'http://localhost:5220';

const API_PUESTOS =
  `${API_URL}/api/Puestos`;

export const gateway = {

  async request(endpoint, options = {}) {
    const url =
      `${API_PUESTOS}${endpoint}`;

    const defaultOptions = {
      headers: {
        'Content-Type': 'application/json',
        ...authService.getAuthHeaders(),
        ...options.headers
      },
      ...options
    };

    try {
      const response =
        await fetch(
          url,
          defaultOptions
        );

      if (response.status === 401) {
        authService.removeToken();

        throw new Error(
          'Sesion expirada. Por favor, inicie sesion nuevamente.'
        );
      }

      const data =
        await response.json();

      if (!response.ok) {
        throw new Error(
          data.mensaje ||
          data.message ||
          'Error en la peticion'
        );
      }

      const paginacion = {
        totalCount: parseInt(
          response.headers.get(
            'X-Total-Count'
          ) || '0'
        ),

        page: parseInt(
          response.headers.get(
            'X-Page'
          ) || '1'
        ),

        pageSize: parseInt(
          response.headers.get(
            'X-Page-Size'
          ) || '10'
        ),

        totalPages: parseInt(
          response.headers.get(
            'X-Total-Pages'
          ) || '0'
        )
      };

      return {
        ...data,
        paginacion
      };

    } catch (error) {
      if (
        error.message.includes(
          'Failed to fetch'
        )
      ) {
        throw new Error(
          'No se pudo conectar al servidor. Verifique su conexion.'
        );
      }

      throw error;
    }
  },

  async obtenerPuestosActivos(
    pagina = 1,
    tamanoPagina = 10
  ) {
    const queryParams =
      new URLSearchParams({
        pagina:
          pagina.toString(),

        tamanoPagina:
          tamanoPagina.toString()
      });

    const response =
      await this.request(
        `/?${queryParams}`,
        {
          method: 'GET'
        }
      );

    console.log(
      'Respuesta de obtenerPuestosActivos:',
      response
    );

    if (
      response.data &&
      response.data.datos
    ) {
      return {
        exito:
          response.codigo === 200,

        mensaje:
          response.mensaje,

        puestos:
          response.data.datos.map(
            item => ({
              idPuesto:
                item.idPuesto || 0,

              codigo:
                item.codigoPuesto || '',

              nombre:
                item.nombrePuesto || ''
            })
          ),

        paginacion:
          response.paginacion
      };
    }

    return {
      exito: false,

      mensaje:
        'No se pudieron obtener los puestos',

      puestos: [],

      paginacion: {
        totalCount: 0,
        page: 1,
        pageSize: 10,
        totalPages: 0
      }
    };
  },

  async obtenerTodosLosPuestosActivos() {
    try {
      const resultado =
        await this.obtenerPuestosActivos(
          1,
          100
        );

      console.log(
        'Resultado de obtenerTodosLosPuestosActivos:',
        resultado
      );

      if (resultado.exito) {
        console.log(
          'Puestos encontrados:',
          resultado.puestos.length
        );

        return resultado;
      }

      console.log(
        'Error:',
        resultado.mensaje
      );

      return resultado;

    } catch (error) {
      console.error(
        'Error al obtener todos los puestos:',
        error
      );

      return {
        exito: false,

        mensaje:
          error.message ||
          'Error al obtener todos los puestos',

        puestos: [],

        paginacion: {
          totalCount: 0,
          page: 1,
          pageSize: 100,
          totalPages: 0
        }
      };
    }
  },

  async login(
    usuario,
    contrasena
  ) {
    try {
      const url =
        `${API_GATEWAY}/gateway/login`;

      console.log(
        'Intentando login por Gateway en:',
        url
      );

      console.log(
        'Usuario:',
        usuario
      );

      const loginRequest = {
        usuario:
          usuario,

        contrasena:
          contrasena
      };

      const response =
        await fetch(
          url,
          {
            method: 'POST',

            headers: {
              'Content-Type':
                'application/json',

              'Accept':
                'application/json'
            },

            body:
              JSON.stringify(
                loginRequest
              )
          }
        );

      const textResponse =
        await response.text();

      if (
        !textResponse ||
        textResponse.trim() === ''
      ) {
        throw new Error(
          'El servidor no respondio. Verifica que el API Gateway y Core4_Login esten corriendo.'
        );
      }

      let data;

      try {
        data =
          JSON.parse(
            textResponse
          );

      } catch (parseError) {
        console.error(
          'Error al parsear JSON:',
          parseError
        );

        throw new Error(
          'La respuesta del servidor no es valida.'
        );
      }

      if (!response.ok) {
        if (
          data &&
          data.mensaje
        ) {
          throw new Error(
            data.mensaje
          );
        }

        throw new Error(
          'Credenciales incorrectas'
        );
      }

      const token =
        data.data?.token ||
        data.token;

      const usuarioData =
        data.data?.usuario ||
        data.usuario ||
        {
          nombre:
            usuario
        };

      if (!token) {
        throw new Error(
          'El servidor no devolvio un token valido.'
        );
      }

      authService.setToken(
        token
      );

      authService.setUser(
        usuarioData
      );

      console.log(
        'Login exitoso por Gateway, token guardado'
      );

      return data;

    } catch (error) {
      console.error(
        'Error en login:',
        error
      );

      throw new Error(
        error.message ||
        'Error al iniciar sesion'
      );
    }
  },

  async validarToken(
    token
  ) {
    try {
      const url =
        `${API_GATEWAY}/gateway/login/validar`;

      console.log(
        'Validando token por Gateway en:',
        url
      );

      const response =
        await fetch(
          url,
          {
            method: 'GET',

            headers: {
              'Authorization':
                'Bearer ' + token,

              'Accept':
                'application/json'
            }
          }
        );

      const textResponse =
        await response.text();

      if (
        !textResponse ||
        textResponse.trim() === ''
      ) {
        throw new Error(
          'El servidor no respondio.'
        );
      }

      let data;

      try {
        data =
          JSON.parse(
            textResponse
          );

      } catch (parseError) {
        console.error(
          'Error al parsear JSON:',
          parseError
        );

        throw new Error(
          'La respuesta del servidor no es valida.'
        );
      }

      if (!response.ok) {
        return {
          valido: false,

          mensaje:
            data.mensaje ||
            'Token invalido'
        };
      }

      return {
        valido:
          data.valido ?? true,

        mensaje:
          data.mensaje ||
          'Token valido',

        data:
          data.data
      };

    } catch (error) {
      console.error(
        'Error al validar token:',
        error
      );

      return {
        valido: false,
        mensaje:
          error.message
      };
    }
  }
};

export default API_GATEWAY;