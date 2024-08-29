import React, { useState } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Home from './Components/Home';
import Login from './Components/Login';
import Register from './Components/Register';
import SolarWatch from './Components/SolarWatch';
import './App.css';

const App = () => {
  const [auth, setAuth] = useState(false);

  const ProtectedRoute = ({ element }) => {
    return auth ? element : <Navigate to="/login" />;
  };

  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login setAuth={setAuth} />} />
        <Route path="/register" element={<Register />} />
        <Route path="/solar-watch" element={<ProtectedRoute element={<SolarWatch />} />} />
      </Routes>
    </div>
  );
};

export default App;
