import { Outlet, useNavigate } from "react-router-dom";
import { Color } from "../components/Notification/Notification";
import { policiesAuthentication } from "../helpers/policiesAuthentication";
import useAuth from "../hooks/useAuth";
import useNotification from "../hooks/useNotification";
import { useEffect, useState } from "react";

const RequireAuth = ({ children }) => {
    const [auth, setAuth] = useAuth();
    const addNotification = useNotification().addNotification;
    const policies = policiesAuthentication(children);
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!auth || !auth.token) {
            setAuth();
            navigate('/login');
            setLoading(false);
            return;
        }

        const currentDate = new Date();
        const tokenExpiresDate = new Date(auth.tokenExpiresDate);
        if (tokenExpiresDate < currentDate) {
            setAuth();
            const notification = { color: Color.error, id: new Date().getTime(), text: 'Poświadczenie wygasło. Zaloguj się ponownie', timeToClose: 5000 };
            addNotification(notification);
            navigate('/login');
            setLoading(false);
            return;
        }
        
        if (policies.length > 0) {
            const hasPermission = auth.claims.permissions.some((permission) =>
                policies.includes(permission)
            );
            if (!hasPermission) {
                navigate('not-found');
                setLoading(false);
                return;
            }
        }

        setLoading(false);
    }, [auth, setAuth, policies, addNotification, navigate]);

    if (loading) {
        return <p>Loading...</p>;
    }

    return <Outlet />;
}

export default RequireAuth