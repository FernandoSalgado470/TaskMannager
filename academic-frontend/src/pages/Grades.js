import React, { useState, useEffect } from 'react';
import gradesService from '../services/gradesService';

const Grades = () => {
  const [grades, setGrades] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingGrade, setEditingGrade] = useState(null);
  const [formData, setFormData] = useState({
    studentId: '',
    subjectId: '',
    academicPeriodId: '',
    gradeCategoryId: '',
    title: '',
    description: '',
    score: '',
    maxScore: '100',
    gradeDate: new Date().toISOString().split('T')[0],
    comments: '',
  });
  const [errors, setErrors] = useState({});

  useEffect(() => {
    loadGrades();
    loadCategories();
  }, []);

  const loadGrades = async () => {
    try {
      setLoading(true);
      const response = await gradesService.getAllGrades();
      if (response.success) {
        setGrades(response.data);
      }
    } catch (error) {
      console.error('Error al cargar calificaciones:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const response = await gradesService.getActiveCategories();
      if (response.success) {
        setCategories(response.data);
      }
    } catch (error) {
      console.error('Error al cargar categorías:', error);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (errors[name]) {
      setErrors((prev) => ({ ...prev, [name]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};
    if (!formData.studentId) newErrors.studentId = 'El ID del estudiante es requerido';
    if (!formData.subjectId) newErrors.subjectId = 'El ID de la materia es requerido';
    if (!formData.academicPeriodId) newErrors.academicPeriodId = 'El ID del período es requerido';
    if (!formData.title) newErrors.title = 'El título es requerido';
    if (!formData.score) newErrors.score = 'La calificación es requerida';
    if (parseFloat(formData.score) > parseFloat(formData.maxScore)) {
      newErrors.score = 'La calificación no puede ser mayor a la calificación máxima';
    }
    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validationErrors = validate();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    try {
      const gradeData = {
        ...formData,
        studentId: parseInt(formData.studentId),
        subjectId: parseInt(formData.subjectId),
        academicPeriodId: parseInt(formData.academicPeriodId),
        gradeCategoryId: formData.gradeCategoryId ? parseInt(formData.gradeCategoryId) : null,
        score: parseFloat(formData.score),
        maxScore: parseFloat(formData.maxScore),
      };

      if (editingGrade) {
        await gradesService.updateGrade(editingGrade.id, gradeData);
        alert('Calificación actualizada exitosamente');
      } else {
        await gradesService.createGrade(gradeData);
        alert('Calificación creada exitosamente');
      }

      setShowModal(false);
      setEditingGrade(null);
      resetForm();
      loadGrades();
    } catch (error) {
      alert(error.response?.data?.message || 'Error al guardar calificación');
    }
  };

  const handleEdit = (grade) => {
    setEditingGrade(grade);
    setFormData({
      studentId: grade.studentId.toString(),
      subjectId: grade.subjectId.toString(),
      academicPeriodId: grade.academicPeriodId.toString(),
      gradeCategoryId: grade.gradeCategoryId?.toString() || '',
      title: grade.title,
      description: grade.description,
      score: grade.score.toString(),
      maxScore: grade.maxScore.toString(),
      gradeDate: grade.gradeDate.split('T')[0],
      comments: grade.comments || '',
    });
    setShowModal(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('¿Estás seguro de eliminar esta calificación?')) {
      try {
        await gradesService.deleteGrade(id);
        alert('Calificación eliminada exitosamente');
        loadGrades();
      } catch (error) {
        alert('Error al eliminar calificación');
      }
    }
  };

  const resetForm = () => {
    setFormData({
      studentId: '',
      subjectId: '',
      academicPeriodId: '',
      gradeCategoryId: '',
      title: '',
      description: '',
      score: '',
      maxScore: '100',
      gradeDate: new Date().toISOString().split('T')[0],
      comments: '',
    });
    setErrors({});
  };

  const openCreateModal = () => {
    setEditingGrade(null);
    resetForm();
    setShowModal(true);
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="text-xl">Cargando calificaciones...</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Gestión de Calificaciones</h1>
        <button
          onClick={openCreateModal}
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition"
        >
          + Nueva Calificación
        </button>
      </div>

      <div className="bg-white rounded-lg shadow overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">ID Estudiante</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Título</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Categoría</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Calificación</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Porcentaje</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Fecha</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Acciones</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {grades.length === 0 ? (
              <tr>
                <td colSpan="7" className="px-6 py-4 text-center text-gray-500">
                  No hay calificaciones registradas
                </td>
              </tr>
            ) : (
              grades.map((grade) => (
                <tr key={grade.id}>
                  <td className="px-6 py-4 whitespace-nowrap">{grade.studentId}</td>
                  <td className="px-6 py-4 whitespace-nowrap font-medium">{grade.title}</td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    {grade.gradeCategory?.name || 'Sin categoría'}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    {grade.score} / {grade.maxScore}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 py-1 rounded ${
                      grade.percentageScore >= 70 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                    }`}>
                      {grade.percentageScore.toFixed(1)}%
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    {new Date(grade.gradeDate).toLocaleDateString()}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <button
                      onClick={() => handleEdit(grade)}
                      className="text-blue-600 hover:text-blue-800 mr-3"
                    >
                      Editar
                    </button>
                    <button
                      onClick={() => handleDelete(grade.id)}
                      className="text-red-600 hover:text-red-800"
                    >
                      Eliminar
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full flex items-center justify-center">
          <div className="bg-white p-8 rounded-lg shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
            <h2 className="text-2xl font-bold mb-4">
              {editingGrade ? 'Editar Calificación' : 'Nueva Calificación'}
            </h2>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">ID Estudiante</label>
                  <input
                    type="number"
                    name="studentId"
                    value={formData.studentId}
                    onChange={handleChange}
                    className={`mt-1 block w-full px-3 py-2 border ${
                      errors.studentId ? 'border-red-300' : 'border-gray-300'
                    } rounded-md`}
                  />
                  {errors.studentId && <p className="text-red-600 text-sm">{errors.studentId}</p>}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700">ID Materia</label>
                  <input
                    type="number"
                    name="subjectId"
                    value={formData.subjectId}
                    onChange={handleChange}
                    className={`mt-1 block w-full px-3 py-2 border ${
                      errors.subjectId ? 'border-red-300' : 'border-gray-300'
                    } rounded-md`}
                  />
                  {errors.subjectId && <p className="text-red-600 text-sm">{errors.subjectId}</p>}
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">ID Período</label>
                  <input
                    type="number"
                    name="academicPeriodId"
                    value={formData.academicPeriodId}
                    onChange={handleChange}
                    className={`mt-1 block w-full px-3 py-2 border ${
                      errors.academicPeriodId ? 'border-red-300' : 'border-gray-300'
                    } rounded-md`}
                  />
                  {errors.academicPeriodId && <p className="text-red-600 text-sm">{errors.academicPeriodId}</p>}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700">Categoría</label>
                  <select
                    name="gradeCategoryId"
                    value={formData.gradeCategoryId}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
                  >
                    <option value="">Sin categoría</option>
                    {categories.map((cat) => (
                      <option key={cat.id} value={cat.id}>
                        {cat.name} ({cat.weightPercentage}%)
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Título</label>
                <input
                  type="text"
                  name="title"
                  value={formData.title}
                  onChange={handleChange}
                  className={`mt-1 block w-full px-3 py-2 border ${
                    errors.title ? 'border-red-300' : 'border-gray-300'
                  } rounded-md`}
                />
                {errors.title && <p className="text-red-600 text-sm">{errors.title}</p>}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Descripción</label>
                <textarea
                  name="description"
                  value={formData.description}
                  onChange={handleChange}
                  rows="3"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
                />
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Calificación</label>
                  <input
                    type="number"
                    step="0.01"
                    name="score"
                    value={formData.score}
                    onChange={handleChange}
                    className={`mt-1 block w-full px-3 py-2 border ${
                      errors.score ? 'border-red-300' : 'border-gray-300'
                    } rounded-md`}
                  />
                  {errors.score && <p className="text-red-600 text-sm">{errors.score}</p>}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700">Máximo</label>
                  <input
                    type="number"
                    step="0.01"
                    name="maxScore"
                    value={formData.maxScore}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700">Fecha</label>
                  <input
                    type="date"
                    name="gradeDate"
                    value={formData.gradeDate}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Comentarios</label>
                <textarea
                  name="comments"
                  value={formData.comments}
                  onChange={handleChange}
                  rows="2"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
                />
              </div>

              <div className="flex justify-end space-x-2 mt-6">
                <button
                  type="button"
                  onClick={() => setShowModal(false)}
                  className="px-4 py-2 bg-gray-300 text-gray-700 rounded-md hover:bg-gray-400"
                >
                  Cancelar
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                >
                  {editingGrade ? 'Actualizar' : 'Crear'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Grades;
