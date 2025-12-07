import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import Subjects from './pages/Subjects';
import Groups from './pages/Groups';
// 🎯 1. CORRECCIÓN: Ahora coincide exactamente con el nombre de archivo 'StudentRegistration.js'
import StudentRegistration from './pages/StudentRegistration'; 

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/subjects" element={<Subjects />} />
          <Route path="/groups" element={<Groups />} />
          
          {/* 🎯 2. Usar el componente importado */}
          <Route 
             path="/students/register" 
             element={<StudentRegistration />} 
          />
        </Routes>
      </div>
    </Router>
  );
}

export default App;