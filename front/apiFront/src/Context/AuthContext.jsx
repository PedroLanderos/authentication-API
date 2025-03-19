import React, { createContext, useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState({
    isAuthenticated: false,
    token: null,
    user: null,
  });

  // 🔹 Función para verificar si hay sesión activa
  const checkSession = () => {
    const token = sessionStorage.getItem("token");
    const user = JSON.parse(sessionStorage.getItem("user"));

    if (token && user) {
      setAuth({
        isAuthenticated: true,
        token,
        user,
      });
    } else {
      setAuth({ isAuthenticated: false, token: null, user: null });
    }
  };

  useEffect(() => {
    checkSession();
  }, []); // Se ejecuta solo una vez al montar el componente

  const login = (token) => {
    if (!token || typeof token !== "string") {
      console.error("Token inválido al intentar decodificar:", token);
      return;
    }

    try {
      const decodedToken = jwtDecode(token);

      console.log("🔍 Token decodificado:", decodedToken);

      const user = {
        id: decodedToken.UserId ? parseInt(decodedToken.UserId, 10) : null,
        name: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || "Usuario",
        email: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || "Sin correo",
        role: decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || "Normal",
      };

      // 🔹 Guardar datos en sessionStorage (NO en localStorage)
      sessionStorage.setItem("token", token);
      sessionStorage.setItem("user", JSON.stringify(user));

      setAuth({
        isAuthenticated: true,
        token,
        user,
      });

      console.log("✅ Usuario autenticado correctamente:", user);
    } catch (error) {
      console.error("❌ Error al decodificar el token:", error);
      logout();
    }
  };

  const logout = () => {
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("user");
    setAuth({ isAuthenticated: false, token: null, user: null });
  };

  return (
    <AuthContext.Provider value={{ auth, login, logout, checkSession }}>
      {children}
    </AuthContext.Provider>
  );
};
