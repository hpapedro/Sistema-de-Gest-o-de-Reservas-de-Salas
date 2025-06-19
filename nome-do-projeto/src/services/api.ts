import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api', // ajuste conforme necessário
});

// Interceptor de requisição para incluir o token automaticamente
api.interceptors.request.use((config) => {
  if (typeof window !== 'undefined') {
    const token = localStorage.getItem('token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }
  return config;
});

// Interceptor de resposta para tratar erro 401
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (typeof window !== 'undefined' && error.response?.status === 401) {
      window.location.href = '/nao-autenticado';
    }
    return Promise.reject(error);
  }
);

export default api;
