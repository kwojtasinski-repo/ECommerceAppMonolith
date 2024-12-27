import { useCallback, useContext } from "react";
import AuthContext from "../context/AuthContext";

export default function useAuth() {
    const authContext = useContext(AuthContext);
    const auth = authContext.user;
    
    const setAuth = useCallback((user) => {
        debugger
        if (user) {
            //login
            authContext.login(user);
            window.localStorage.setItem('token-data', JSON.stringify(user));
        } else {
            // logout
            authContext.logout();
            console.log('logout');
            window.localStorage.removeItem('token-data');
        }
    }, [authContext]);

    return [auth, setAuth];
};
