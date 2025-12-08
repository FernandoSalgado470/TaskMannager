import axios from 'axios';

const API_URL = 'http://localhost:5002/api';

const gradesServiceAPI = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add token
gradesServiceAPI.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor to handle errors
gradesServiceAPI.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

const gradesService = {
  // Grades endpoints
  getAllGrades: async () => {
    const response = await gradesServiceAPI.get('/grades');
    return response.data;
  },

  getGradeById: async (id) => {
    const response = await gradesServiceAPI.get(`/grades/${id}`);
    return response.data;
  },

  getGradesByStudentId: async (studentId) => {
    const response = await gradesServiceAPI.get(`/grades/student/${studentId}`);
    return response.data;
  },

  getGradesBySubjectId: async (subjectId) => {
    const response = await gradesServiceAPI.get(`/grades/subject/${subjectId}`);
    return response.data;
  },

  getGradesByStudentAndSubject: async (studentId, subjectId) => {
    const response = await gradesServiceAPI.get(`/grades/student/${studentId}/subject/${subjectId}`);
    return response.data;
  },

  createGrade: async (gradeData) => {
    const response = await gradesServiceAPI.post('/grades', gradeData);
    return response.data;
  },

  updateGrade: async (id, gradeData) => {
    const response = await gradesServiceAPI.put(`/grades/${id}`, gradeData);
    return response.data;
  },

  deleteGrade: async (id) => {
    const response = await gradesServiceAPI.delete(`/grades/${id}`);
    return response.data;
  },

  // Grade Categories endpoints
  getAllCategories: async () => {
    const response = await gradesServiceAPI.get('/gradecategories');
    return response.data;
  },

  getActiveCategories: async () => {
    const response = await gradesServiceAPI.get('/gradecategories/active');
    return response.data;
  },

  getCategoryById: async (id) => {
    const response = await gradesServiceAPI.get(`/gradecategories/${id}`);
    return response.data;
  },

  createCategory: async (categoryData) => {
    const response = await gradesServiceAPI.post('/gradecategories', categoryData);
    return response.data;
  },

  updateCategory: async (id, categoryData) => {
    const response = await gradesServiceAPI.put(`/gradecategories/${id}`, categoryData);
    return response.data;
  },

  deleteCategory: async (id) => {
    const response = await gradesServiceAPI.delete(`/gradecategories/${id}`);
    return response.data;
  },

  // Student Grades endpoints
  getAllStudentGrades: async () => {
    const response = await gradesServiceAPI.get('/studentgrades');
    return response.data;
  },

  getStudentGradeById: async (id) => {
    const response = await gradesServiceAPI.get(`/studentgrades/${id}`);
    return response.data;
  },

  getStudentGradesByStudentId: async (studentId) => {
    const response = await gradesServiceAPI.get(`/studentgrades/student/${studentId}`);
    return response.data;
  },

  getStudentGradesBySubjectId: async (subjectId) => {
    const response = await gradesServiceAPI.get(`/studentgrades/subject/${subjectId}`);
    return response.data;
  },

  createStudentGrade: async (gradeData) => {
    const response = await gradesServiceAPI.post('/studentgrades', gradeData);
    return response.data;
  },

  updateStudentGrade: async (id, gradeData) => {
    const response = await gradesServiceAPI.put(`/studentgrades/${id}`, gradeData);
    return response.data;
  },

  deleteStudentGrade: async (id) => {
    const response = await gradesServiceAPI.delete(`/studentgrades/${id}`);
    return response.data;
  },

  calculateFinalGrade: async (studentId, subjectId, periodId) => {
    const response = await gradesServiceAPI.post(
      `/studentgrades/calculate?studentId=${studentId}&subjectId=${subjectId}&periodId=${periodId}`
    );
    return response.data;
  },
};

export default gradesService;
