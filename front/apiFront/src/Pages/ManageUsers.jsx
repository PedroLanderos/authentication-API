import React, { useState, useEffect, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import API_BASE_URL from "../config/apiConfig"; // ✅ URL Base
import "./CSS/ManageUsers.css"; // ✅ Se mantiene el CSS

const ManageUsers = () => {
  const { auth } = useContext(AuthContext);
  const [users, setUsers] = useState([]);
  const [editingUser, setEditingUser] = useState(null);
  const [message, setMessage] = useState("");

  const fetchUsers = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/authenticationapi`, {
        headers: { Authorization: `Bearer ${auth.token}` },
      });
      console.log("🔹 Usuarios obtenidos:", response.data); // 🛠️ Depuración
      setUsers(response.data);
    } catch (error) {
      console.error("❌ Error al obtener los usuarios:", error);
      setMessage("Error al obtener los usuarios.");
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const handleEditClick = (user) => {
    setEditingUser({
      ...user,
      password: "", // ✅ Permitir edición de la contraseña
    });
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditingUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleUpdateSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.put(`${API_BASE_URL}/AuthenticationApi`, editingUser, {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${auth.token}`,
        },
      });
      if (response.status === 200 || response.status === 201) {
        setMessage("✅ Usuario actualizado exitosamente.");
        setEditingUser(null);
        fetchUsers(); // ✅ Recargar la lista
      }
    } catch (error) {
      console.error("❌ Error al actualizar el usuario:", error);
      setMessage("Error al actualizar el usuario. Intenta de nuevo.");
    }
  };

  return (
    <div className="manage-users">
      <h1>Administrar Usuarios</h1>
      {message && (
        <p
          style={{
            color: message.includes("Error") ? "red" : "green",
            textAlign: "center",
          }}
        >
          {message}
        </p>
      )}

      {!editingUser ? (
        <div className="user-table-container">
          <table className="user-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Teléfono</th>
                <th>Dirección</th>
                <th>Email</th>
                <th>Rol</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {users.map((user) => (
                <tr key={user.id}>
                  <td>{user.id}</td>
                  <td>{user.name}</td>
                  <td>{user.telephoneNumber}</td>
                  <td>{user.address}</td>
                  <td>{user.email}</td>
                  <td>{user.role}</td>
                  <td>
                    <button className="edit-btn" onClick={() => handleEditClick(user)}>
                      ✏️ Editar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <div className="edit-user-container">
          <h2>Editar Usuario</h2>
          <form onSubmit={handleUpdateSubmit} className="edit-user-form">
            <label>
              Nombre:
              <input
                type="text"
                name="name"
                value={editingUser.name}
                onChange={handleInputChange}
                required
              />
            </label>
            <label>
              Teléfono:
              <input
                type="tel"
                name="telephoneNumber"
                value={editingUser.telephoneNumber}
                onChange={handleInputChange}
                required
              />
            </label>
            <label>
              Dirección:
              <input
                type="text"
                name="address"
                value={editingUser.address}
                onChange={handleInputChange}
                required
              />
            </label>
            <label>
              Email:
              <input
                type="email"
                name="email"
                value={editingUser.email}
                onChange={handleInputChange}
                required
              />
            </label>
            <label>
              Contraseña:
              <input
                type="password"
                name="password"
                value={editingUser.password}
                onChange={handleInputChange}
              />
            </label>
            <label>
              Rol:
              <select
                name="role"
                value={editingUser.role}
                onChange={handleInputChange}
                required
              >
                <option value="Normal">Normal</option>
                <option value="Admin">Admin</option>
              </select>
            </label>
            <button type="submit">💾 Guardar Cambios</button>
            <button type="button" className="cancel-btn" onClick={() => setEditingUser(null)}>
              ❌ Cancelar
            </button>
          </form>
        </div>
      )}
    </div>
  );
};

export default ManageUsers;
