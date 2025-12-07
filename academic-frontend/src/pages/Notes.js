import React, { useState } from "react";

const MAX_CLASSES = 5;

const Notes = () => {
  const [notes, setNotes] = useState([
    { id: 1, subject: "Clase 1", note: "" },
  ]);

  const handleChange = (id, field, value) => {
    setNotes((prev) =>
      prev.map((n) => (n.id === id ? { ...n, [field]: value } : n))
    );
  };

  const handleAddRow = () => {
    if (notes.length >= MAX_CLASSES) return;

    setNotes((prev) => [
      ...prev,
      {
        id: Date.now(),
        subject: `Clase ${prev.length + 1}`,
        note: "",
      },
    ]);
  };

  const handleRemoveRow = (id) => {
    setNotes((prev) => prev.filter((n) => n.id !== id));
  };

  const parseNote = (n) => {
    const value = parseFloat(n);
    if (isNaN(value)) return 0;
    // aquí puedes decidir si tu nota es de 0 a 5, 0 a 10 o 0 a 100
    // por ahora asumimos 0–100
    return Math.min(Math.max(value, 0), 100);
  };

  const average =
    notes.length === 0
      ? 0
      : notes.reduce((sum, n) => sum + parseNote(n.note), 0) / notes.length;

  return (
    <div className="max-w-5xl mx-auto px-4 py-6">
      <h1 className="text-3xl font-bold mb-2">Notas de clase</h1>
      <p className="text-gray-600 mb-6">
        Registra las notas de tus clases (máximo {MAX_CLASSES}) y mira el
        resumen visual.
      </p>

      {/* Resumen */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
        <div className="bg-white rounded-xl shadow p-4">
          <h2 className="text-sm font-semibold text-gray-500">Total de clases</h2>
          <p className="text-3xl font-bold mt-2">{notes.length}</p>
        </div>

        <div className="bg-white rounded-xl shadow p-4">
          <h2 className="text-sm font-semibold text-gray-500">Promedio</h2>
          <p className="text-3xl font-bold mt-2">
            {average.toFixed(1)}
          </p>
          <p className="text-xs text-gray-400 mt-1">(escala 0 – 100)</p>
        </div>

        <div className="bg-white rounded-xl shadow p-4">
          <h2 className="text-sm font-semibold text-gray-500">Clases restantes</h2>
          <p className="text-3xl font-bold mt-2">
            {MAX_CLASSES - notes.length}
          </p>
          <p className="text-xs text-gray-400 mt-1">
            Puedes registrar hasta {MAX_CLASSES} clases.
          </p>
        </div>
      </div>

      {/* Tabla de notas */}
      <div className="bg-white rounded-xl shadow p-4 mb-4">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-lg font-semibold">Listado de notas</h2>
          <button
            onClick={handleAddRow}
            disabled={notes.length >= MAX_CLASSES}
            className={`px-4 py-2 rounded-lg text-sm font-semibold ${
              notes.length >= MAX_CLASSES
                ? "bg-gray-300 text-gray-600 cursor-not-allowed"
                : "bg-blue-600 text-white hover:bg-blue-700"
            }`}
          >
            + Agregar clase
          </button>
        </div>

        {notes.length === 0 ? (
          <p className="text-gray-500 text-sm">
            No hay clases registradas todavía.
          </p>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full text-sm">
              <thead>
                <tr className="text-left border-b">
                  <th className="py-2 pr-4">Clase</th>
                  <th className="py-2 pr-4">Nota (0–100)</th>
                  <th className="py-2 pr-4">Visual</th>
                  <th className="py-2 text-right">Acciones</th>
                </tr>
              </thead>
              <tbody>
                {notes.map((row) => {
                  const value = parseNote(row.note);
                  return (
                    <tr key={row.id} className="border-b last:border-b-0">
                      <td className="py-2 pr-4">
                        <input
                          type="text"
                          className="w-full border rounded-lg px-2 py-1 text-sm"
                          value={row.subject}
                          onChange={(e) =>
                            handleChange(row.id, "subject", e.target.value)
                          }
                          placeholder="Nombre de la clase"
                        />
                      </td>
                      <td className="py-2 pr-4">
                        <input
                          type="number"
                          min="0"
                          max="100"
                          className="w-24 border rounded-lg px-2 py-1 text-sm"
                          value={row.note}
                          onChange={(e) =>
                            handleChange(row.id, "note", e.target.value)
                          }
                          placeholder="0–100"
                        />
                      </td>
                      <td className="py-2 pr-4">
                        <div className="w-full bg-gray-200 rounded-full h-3">
                          <div
                            className="h-3 rounded-full bg-blue-500 transition-all"
                            style={{ width: `${value}%` }}
                          />
                        </div>
                        <span className="text-xs text-gray-500">
                          {value.toFixed(0)}%
                        </span>
                      </td>
                      <td className="py-2 text-right">
                        <button
                          onClick={() => handleRemoveRow(row.id)}
                          className="text-xs text-red-500 hover:text-red-700"
                        >
                          Eliminar
                        </button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <p className="text-xs text-gray-400">
        * Por ahora estas notas se guardan solo en memoria del navegador (si
        recargas la página se reinician). Si luego quieres, hacemos un API para
        guardarlas en la base de datos.
      </p>
    </div>
  );
};

export default Notes;
