import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Subjects from "./pages/Subjects";
import Groups from "./pages/Groups";
import Notes from "./pages/Notes"; // 👈 nueva página

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/subjects" element={<Subjects />} />
          <Route path="/groups" element={<Groups />} />
          <Route path="/notes" element={<Notes />} /> {/* 👈 nueva ruta */}
        </Routes>
      </div>
    </Router>
  );
}

export default App;
