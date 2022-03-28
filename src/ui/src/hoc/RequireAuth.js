import { Navigate } from "react-router-dom";
import { Color } from "../components/Notification/Notification";
import { policiesAuthentication } from "../helpers/policiesAuthentication";
import useAuth from "../hooks/useAuth";
import useNotification from "../hooks/useNotification";
import NotFound from "../pages/404/NotFound";

const RequireAuth = ({ children }) => {
    const [auth, setAuth] = useAuth();
    const [notifications, addNotification] = useNotification();

    const policies = policiesAuthentication(children);
    let response = auth ? children : (<Navigate to="/login" />);
    const currentDate = new Date();

    if (auth) {
        const tokenExpiresDate = new Date(auth.tokenExpiresDate);
        
        if (tokenExpiresDate < currentDate) {
            setAuth();
            const notification = { color: Color.error, id: new Date().getTime(), text: 'Poświadczenie wygasło. Zaloguj się ponownie', timeToClose: 5000 };
            addNotification(notification);
            return (<Navigate to = "/login" />);
        }
    }

    if (auth && policies.length > 0) {
        response = auth.claims.permissions.some(p => policies.includes(p)) ? children : (<NotFound/>);
    }

    return response;
}

export default RequireAuth