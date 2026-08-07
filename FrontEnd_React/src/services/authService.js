// src/services/authService.js

const TOKEN_KEY = 'auth_token';
const USER_KEY = 'usuario_data';

export const authService = {
  setToken(token) {
    localStorage.setItem(TOKEN_KEY, token);
  },

  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  },

  removeToken() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    localStorage.removeItem('nombreUsuario');
  },

  setUser(userData) {
    localStorage.setItem(USER_KEY, JSON.stringify(userData));
    if (userData.nombre || userData.Nombre) {
      localStorage.setItem('nombreUsuario', userData.nombre || userData.Nombre);
    }
  },

  getUser() {
    const data = localStorage.getItem(USER_KEY);
    return data ? JSON.parse(data) : null;
  },

  isAuthenticated() {
    return !!this.getToken();
  },

  getAuthHeaders() {
    const token = this.getToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
  },

  getNombreUsuario() {
    const user = this.getUser();
    if (user) {
      return user.nombre || user.Nombre || user.usuario || user.Usuario || 'Usuario';
    }
    return localStorage.getItem('nombreUsuario') || 'Usuario';
  },

  getInicialUsuario() {
    const nombre = this.getNombreUsuario();
    return nombre.charAt(0).toUpperCase();
  }
};