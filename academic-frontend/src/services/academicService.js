import { academicServiceAPI } from '../utils/api';

const academicService = {
  // SUBJECTS
  getAllSubjects: async () => {
    const response = await academicServiceAPI.get('/subjects');
    return response.data;
  },

  getSubjectById: async (id) => {
    const response = await academicServiceAPI.get(`/subjects/${id}`);
    return response.data;
  },

  createSubject: async (subjectData) => {
    const response = await academicServiceAPI.post('/subjects', subjectData);
    return response.data;
  },

  updateSubject: async (id, updateData) => {
    const response = await academicServiceAPI.put(`/subjects/${id}`, updateData);
    return response.data;
  },

  deleteSubject: async (id) => {
    const response = await academicServiceAPI.delete(`/subjects/${id}`);
    return response.data;
  },

  // GROUPS
  getAllGroups: async () => {
    const response = await academicServiceAPI.get('/groups');
    return response.data;
  },

  getGroupById: async (id) => {
    const response = await academicServiceAPI.get(`/groups/${id}`);
    return response.data;
  },

  getGroupsByLevel: async (level) => {
    const response = await academicServiceAPI.get(`/groups/level/${level}`);
    return response.data;
  },

  getActiveGroups: async () => {
    const response = await academicServiceAPI.get('/groups/active');
    return response.data;
  },

  createGroup: async (groupData) => {
    const response = await academicServiceAPI.post('/groups', groupData);
    return response.data;
  },

  updateGroup: async (id, updateData) => {
    const response = await academicServiceAPI.put(`/groups/${id}`, updateData);
    return response.data;
  },

  deleteGroup: async (id) => {
    const response = await academicServiceAPI.delete(`/groups/${id}`);
    return response.data;
  },

  // ACADEMIC PERIODS
  getAllPeriods: async () => {
    const response = await academicServiceAPI.get('/academic-periods');
    return response.data;
  },

  getCurrentPeriod: async () => {
    const response = await academicServiceAPI.get('/academic-periods/current');
    return response.data;
  },

  createPeriod: async (periodData) => {
    const response = await academicServiceAPI.post('/academic-periods', periodData);
    return response.data;
  },

  activatePeriod: async (id) => {
    const response = await academicServiceAPI.post(`/academic-periods/${id}/activate`);
    return response.data;
  },

  // STUDENT ENROLLMENTS
  getAllEnrollments: async () => {
    const response = await academicServiceAPI.get('/student-enrollments');
    return response.data;
  },

  getEnrollmentsByStudent: async (studentId) => {
    const response = await academicServiceAPI.get(`/student-enrollments/student/${studentId}`);
    return response.data;
  },

  getEnrollmentsByGroup: async (groupId) => {
    const response = await academicServiceAPI.get(`/student-enrollments/group/${groupId}`);
    return response.data;
  },

  enrollStudent: async (enrollmentData) => {
    const response = await academicServiceAPI.post('/student-enrollments', enrollmentData);
    return response.data;
  },

  // TEACHER ASSIGNMENTS
  getAllAssignments: async () => {
    const response = await academicServiceAPI.get('/teacher-assignments');
    return response.data;
  },

  getAssignmentsByTeacher: async (teacherId) => {
    const response = await academicServiceAPI.get(`/teacher-assignments/teacher/${teacherId}`);
    return response.data;
  },

  getAssignmentsByGroup: async (groupId) => {
    const response = await academicServiceAPI.get(`/teacher-assignments/group/${groupId}`);
    return response.data;
  },

  createAssignment: async (assignmentData) => {
    const response = await academicServiceAPI.post('/teacher-assignments', assignmentData);
    return response.data;
  },
};

export default academicService;
