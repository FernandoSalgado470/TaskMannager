import React from 'react';
import { Navigate } from 'react-router-dom';
import loginService from '../services/loginService';

const ProtectedRoute = ({ children }) => {
  const isAuthenticated = loginService.isAuthenticated();

  if (!isAuthenticated) {
    // Redirigir al login si no está autenticado
    return <Navigate to="/login" replace />;
  }

  // Si está autenticado, mostrar el contenido
  return children;
};

export default ProtectedRoute;
