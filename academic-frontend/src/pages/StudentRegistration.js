import React, { useState } from 'react';

// URL base de tu API que está corriendo en http://localhost:5003
const API_BASE_URL = 'http://localhost:5003/api';

const StudentRegistration = () => {
    // 1. Estado para los datos del estudiante
    const [studentData, setStudentData] = useState({
        firstName: '',
        lastName: '',
        email: '',
    });
    // 2. Estado para la matrícula (asociación al grupo)
    const [groupId, setGroupId] = useState('');
    const [message, setMessage] = useState('');
    const [loading, setLoading] = useState(false);

    // Maneja los cambios en los inputs del estudiante
    const handleStudentChange = (e) => {
        setStudentData({
            ...studentData,
            [e.target.name]: e.target.value,
        });
    };

    // Maneja la creación del estudiante y la matrícula
    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setLoading(true);

        // Validaciones básicas
        if (!studentData.firstName || !studentData.email || !groupId) {
            setMessage('Error: Complete todos los campos, incluyendo el Grupo.');
            setLoading(false);
            return;
        }

        let newStudentId = null;

        try {
            // --- PASO 1: CREAR EL ESTUDIANTE (POST /api/students) ---
            const studentResponse = await fetch(`${API_BASE_URL}/students`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    // Si usas AuthContext.js, podrías necesitar un token de autorización aquí.
                },
                body: JSON.stringify(studentData),
            });

            if (!studentResponse.ok) {
                const errorBody = await studentResponse.json();
                throw new Error(`Error al crear estudiante: ${errorBody.title || studentResponse.statusText}`);
            }

            const newStudent = await studentResponse.json();
            // 🎯 CORRECCIÓN CLAVE: Asegurarse de que el ID del estudiante se capture correctamente.
            newStudentId = newStudent.id || newStudent.Id; // Usar 'id' o 'Id' según como lo devuelva el C#
            
            if (!newStudentId) {
                 throw new Error("El API creó el estudiante, pero no devolvió un ID válido.");
            }
            
            // --- PASO 2: MATRICULAR AL ESTUDIANTE (POST /api/studentgroups) ---
            const enrollmentData = {
                studentId: newStudentId, // Usamos el ID capturado
                groupId: parseInt(groupId),
                enrollmentDate: new Date().toISOString(),
            };
            
            const enrollmentResponse = await fetch(`${API_BASE_URL}/studentgroups`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(enrollmentData),
            });

            if (!enrollmentResponse.ok) {
                 // El estudiante se creó, pero la matrícula falló
                const enrollmentErrorBody = await enrollmentResponse.json();
                setMessage(`Estudiante creado (ID: ${newStudentId}), pero falló la matrícula en el grupo ${groupId}. Error: ${enrollmentErrorBody.title || enrollmentResponse.statusText}`);
                return;
            }

            // 🎯 MENSAJE DE ÉXITO DE AMBOS PASOS
            setMessage(`✅ ¡Estudiante "${newStudent.firstName} ${newStudent.lastName}" (ID: ${newStudentId}) creado y matriculado en el Grupo ${groupId} con éxito!`);
            setStudentData({ firstName: '', lastName: '', email: '' }); // Limpiar formulario
            setGroupId('');

        } catch (error) {
            console.error('Error en el proceso de registro:', error);
            setMessage(`❌ Error: ${error.message}. Asegúrate de que el API esté corriendo y el Grupo ${groupId} exista.`);
        } finally {
            setLoading(false);
        }
    };

    // Tailwind CSS classes for aesthetics
    return (
        <div className="max-w-4xl mx-auto p-6 bg-white rounded-xl shadow-2xl mt-10">
            <h2 className="text-3xl font-extrabold text-blue-800 mb-6 border-b pb-2">
                Registro y Matrícula de Estudiante
            </h2>
            <form onSubmit={handleSubmit} className="space-y-6">
                
                {/* Student Data Fields */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 bg-gray-50 p-4 rounded-lg border">
                    <div>
                        <label htmlFor="firstName" className="block text-sm font-medium text-gray-700">Nombre</label>
                        <input
                            type="text"
                            name="firstName"
                            id="firstName"
                            value={studentData.firstName}
                            onChange={handleStudentChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>
                    <div>
                        <label htmlFor="lastName" className="block text-sm font-medium text-gray-700">Apellido</label>
                        <input
                            type="text"
                            name="lastName"
                            id="lastName"
                            value={studentData.lastName}
                            onChange={handleStudentChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>
                    <div className="col-span-1 md:col-span-2">
                        <label htmlFor="email" className="block text-sm font-medium text-gray-700">Correo Electrónico</label>
                        <input
                            type="email"
                            name="email"
                            id="email"
                            value={studentData.email}
                            onChange={handleStudentChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>
                </div>

                {/* Enrollment Field */}
                <div className="pt-4 border-t mt-6">
                    <h3 className="text-xl font-semibold text-gray-800 mb-4">Detalles de Matrícula</h3>
                    <div>
                        <label htmlFor="groupId" className="block text-sm font-medium text-gray-700">ID del Grupo/Clase (Ej: 1)</label>
                        <input
                            type="number"
                            name="groupId"
                            id="groupId"
                            value={groupId}
                            onChange={(e) => setGroupId(e.target.value)}
                            required
                            min="1"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder="Ingrese el ID del grupo donde se matriculará"
                        />
                    </div>
                </div>

                {/* Status Message */}
                {message && (
                    <div className={`p-4 rounded-md ${message.startsWith('✅') ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                        {message}
                    </div>
                )}

                {/* Submit Button */}
                <div className="flex justify-end">
                    <button
                        type="submit"
                        disabled={loading}
                        className={`inline-flex justify-center py-3 px-6 border border-transparent text-lg font-medium rounded-md shadow-sm text-white transition-colors ${loading ? 'bg-gray-400' : 'bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500'}`}
                    >
                        {loading ? 'Procesando...' : 'Crear Estudiante y Matricular'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default StudentRegistration;