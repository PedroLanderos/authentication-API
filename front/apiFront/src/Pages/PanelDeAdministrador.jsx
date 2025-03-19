import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './CSS/PanelDeAdministrador.css';
import AddUser from './AddUser';
import ManageUsers from './ManageUsers';
import { AuthContext } from '../Context/AuthContext';

const PanelDeAdministrador = () => {
  const [activeTab, setActiveTab] = useState('');
  const [activeSubmenu, setActiveSubmenu] = useState('');
  const { auth, checkSession } = useContext(AuthContext);
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    checkSession(); // Verificar sesión al cargar el componente
  }, []);

  useEffect(() => {
    if (auth.user) {
      if (auth.user.role !== 'Admin') {
        alert('No tienes permiso para acceder a este panel.');
        navigate('/MainPage'); // Redirigir a MainPage si no es admin
      }
      setLoading(false);
    }
  }, [auth.user, navigate]);

  if (loading) {
    return <div className="loading">Cargando...</div>;
  }

  if (!auth.user || auth.user.role !== 'Admin') {
    return null;
  }

  const toggleSubmenu = (menu) => {
    setActiveSubmenu((prev) => (prev === menu ? '' : menu));
  };

  const renderContent = () => {
    switch (activeTab) {
      case 'agregarUsuario':
        return <AddUser />;
      case 'administrarUsuarios':
        return <ManageUsers />;
      default:
        return <div className="panel-placeholder">Selecciona una sección del panel.</div>;
    }
  };

  return (
    <div className="admin-panel">
      <div className="sidebar">
        <h1>Panel de Administrador</h1>
        <ul>
          <li 
            onClick={() => toggleSubmenu('usuarios')} 
            className={activeSubmenu === 'usuarios' ? 'active' : ''}
          >
            Usuarios
            <ul className={`submenu ${activeSubmenu === 'usuarios' ? 'show' : ''}`}>
              <li onClick={() => setActiveTab('agregarUsuario')}>Agregar Usuario</li>
              <li onClick={() => setActiveTab('administrarUsuarios')}>Administrar Usuarios</li>
            </ul>
          </li>
        </ul>
      </div>
      <div className="content">{renderContent()}</div>
    </div>
  );
};

export default PanelDeAdministrador;
