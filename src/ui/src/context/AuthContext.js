import React from 'react';

const AuthContext = React.createContext({
    user: null,
    login: () => {},
    logout: () => {},
    intializing: true
});

AuthContext.displayName = "AuthContext";

export default AuthContext;