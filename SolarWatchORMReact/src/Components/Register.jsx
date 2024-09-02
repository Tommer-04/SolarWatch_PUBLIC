import React, { useState } from 'react';
import { useNavigate, Navigate } from 'react-router-dom';

const Register = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [errorMessages, setErrorMessages] = useState([]);
  const [isRegistered, setIsRegistered] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMessages([]); // Clear previous errors

    const user = {
      userName: username,
      email: email,
      password: password,
      confirmPassword: confirmPassword,
    };

    try {
      const response = await fetch('/api/Auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(user),
      });

      const data = await response.json();

      if (response.status === 200) {
        alert('Registration successful');
        setIsRegistered(true); // Trigger navigation to the main page
      } else if (response.status === 400) {
        handleErrors(data);
      } else {
        alert('Registration failed');
      }
    } catch (error) {
      console.error('Error:', error);
    }
  };

  const handleErrors = (data) => {
    const messages = [];

    if (data.errors) {
      for (const key in data.errors) {
        if (data.errors[key]) {
          messages.push(...data.errors[key]);
        }
      }
    } else {
      // Handle other error structures
      for (const key in data) {
        if (data[key]) {
          messages.push(data[key]);
        }
      }
    }

    setErrorMessages(messages);
  };

  const goBack = () => {
    navigate('/');
  };

  if (isRegistered) {
    return <Navigate to="/" />;
  }

  return (
    <div className="container">
      <h1>Register</h1>
      <form onSubmit={handleSubmit}>
        <label>
          Username:
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </label>
        <label>
          Email:
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </label>
        <label>
          Password:
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </label>
        <label>
          Confirm Password:
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </label>
        <button type="submit">Register</button>
      </form>
      
      {errorMessages.length > 0 && (
        <div className="error-messages">
          <ul>
            {errorMessages.map((message, index) => (
              <li className="error" key={index}>{message}</li>
            ))}
          </ul>
        </div>
      )}

      <button className='goback-button' onClick={goBack}>Go Back</button>
    </div>
  );
};

export default Register;
