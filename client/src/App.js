import './App.css';
import 'semantic-ui-css/semantic.min.css';

import AuthRoute from './util/AuthRoute';
import { AuthProvider } from './context/auth';

import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MenuBar from './components/MenuBar';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import { Container } from 'semantic-ui-react';
import React from 'react';
import SinglePost from './pages/SinglePost';


function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Container>
          <MenuBar />
          <Routes>
            <Route path="/" index element={<Home />} />
            <Route path="/login" element={<AuthRoute element={<Login />} />} />
            <Route path="/register" element={<AuthRoute element={<Register />} />} />
            <Route path='/post/:postId' element={<SinglePost />}/>
          </Routes>
        </Container>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
