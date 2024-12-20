import { Navigate, Outlet } from "react-router-dom";
import { Color } from "../components/Notification/Notification";
import { policiesAuthentication } from "../helpers/policiesAuthentication";
import useAuth from "../hooks/useAuth";
import useNotification from "../hooks/useNotification";
import NotFound from "../pages/404/NotFound";

const RequireAuth = ({ children }) => {
    const [auth, setAuth] = useAuth();
    const addNotification = useNotification().addNotification;
    const policies = policiesAuthentication(children);
    const currentDate = new Date();

    if (!auth) {
        return <Navigate to="/login" />;
    }

    const tokenExpiresDate = new Date(auth.tokenExpiresDate);
    if (tokenExpiresDate < currentDate) {
        setAuth();
        const notification = { color: Color.error, id: new Date().getTime(), text: 'Poświadczenie wygasło. Zaloguj się ponownie', timeToClose: 5000 };
        addNotification(notification);
        return <Navigate to = "/login" />;
    }

    if (policies.length > 0) {
        const hasPermission = auth.claims.permissions.some((permission) =>
            policies.includes(permission)
        );
        if (!hasPermission) {
            return <NotFound />;
        }
    }

    return <Outlet />;
}

export default RequireAuth