import { useContext } from "react";
import AuthContext from "../context/AuthContext";

export default function useAuth() {
    const authContext = useContext(AuthContext);
    const auth = authContext.user ? authContext.user : parseTokenData();
    
    const setAuth = (user) => {
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
    }

    return [auth, setAuth];
};

const parseTokenData = () => {
    try {
        return JSON.parse(window.localStorage.getItem('token-data'));
    } catch {
        return null;
    }
}
