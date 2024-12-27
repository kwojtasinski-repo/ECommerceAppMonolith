import { useContext } from "react";
import AuthContext from "../context/AuthContext";
import { tokenData } from "../constants";

export default function useAuth() {
    const authContext = useContext(AuthContext);
    const auth = authContext.user;
    
    const setAuth = (user) => {
        if (user) {
            //login
            authContext.login(user);
            window.localStorage.setItem(tokenData, JSON.stringify(user));
        } else {
            // logout
            authContext.logout();
            console.log('logout');
            window.localStorage.removeItem(tokenData);
        }
    };

    return [auth, setAuth];
};
