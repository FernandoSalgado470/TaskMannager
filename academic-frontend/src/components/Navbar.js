import React from "react";
import { Link } from "react-router-dom";

const Navbar = () => {
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
          to="/"
          style={{
            color: "white",
            fontSize: "1.5rem",
            fontWeight: "bold",
            textDecoration: "none",
          }}
        >
          TaskClass
        </Link>

        <div style={{ display: "flex", gap: "1rem" }}>
          <Link
            to="/"
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
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
