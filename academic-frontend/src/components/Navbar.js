import React from "react";
import { Link, useNavigate } from "react-router-dom";
import loginService from "../services/loginService";

const Navbar = () => {
  const navigate = useNavigate();
  const isAuthenticated = loginService.isAuthenticated();
  const currentUser = loginService.getCurrentUser();

  const handleLogout = async () => {
    try {
      await loginService.logout();
      navigate("/login");
    } catch (error) {
      console.error("Error al cerrar sesión:", error);
      // Forzar limpieza y redirección incluso si hay error
      localStorage.clear();
      navigate("/login");
    }
  };

  return (
    <nav
      style={{
        backgroundColor: "#2563EB",
        padding: "1rem",
        marginBottom: "2rem",
      }}
    >
      <div
        style={{
          maxWidth: "1200px",
          margin: "0 auto",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Link
          to={isAuthenticated ? "/home" : "/"}
          style={{
            color: "white",
            fontSize: "1.5rem",
            fontWeight: "bold",
            textDecoration: "none",
          }}
        >
          TaskClass
        </Link>

        <div style={{ display: "flex", gap: "1rem", alignItems: "center" }}>
          {isAuthenticated ? (
            <>
              {/* Enlaces visibles solo para usuarios autenticados */}
              <Link
                to="/home"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Inicio
              </Link>

              <Link
                to="/subjects"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Materias
              </Link>

              <Link
                to="/groups"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Grupos
              </Link>

              <Link
                to="/students"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Estudiantes
              </Link>

              <Link
                to="/notes"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Notas
              </Link>

              <Link
                to="/grades"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Calificaciones
              </Link>

              {/* Información del usuario */}
              {currentUser && (
                <span style={{ color: "white", marginLeft: "1rem" }}>
                  {currentUser.fullName || currentUser.email}
                </span>
              )}

              {/* Botón de Logout */}
              <button
                onClick={handleLogout}
                style={{
                  color: "white",
                  backgroundColor: "#DC2626",
                  border: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                  cursor: "pointer",
                  fontWeight: "500",
                }}
              >
                Cerrar Sesión
              </button>
            </>
          ) : (
            <>
              {/* Enlaces visibles solo para usuarios NO autenticados */}
              <Link
                to="/login"
                style={{
                  color: "white",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Iniciar Sesión
              </Link>

              <Link
                to="/register"
                style={{
                  color: "white",
                  backgroundColor: "#059669",
                  textDecoration: "none",
                  padding: "0.5rem 1rem",
                  borderRadius: "0.5rem",
                }}
              >
                Registrarse
              </Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
