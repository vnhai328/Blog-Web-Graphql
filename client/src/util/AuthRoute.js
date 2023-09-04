import React, { useContext } from "react";
import { Navigate } from "react-router-dom";

import { AuthContext } from "../context/auth";

function AuthRoute({element, ...rest }) {
  const { user } = useContext(AuthContext);
  console.log({...rest})
  return user ? <Navigate to="/" /> : element
}


export default AuthRoute