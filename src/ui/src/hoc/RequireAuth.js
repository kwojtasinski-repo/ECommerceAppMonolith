import { Outlet, useNavigate } from "react-router";
import { Color } from "../components/Notification/Notification";
import useAuth from "../hooks/useAuth";
import useNotification from "../hooks/useNotification";
import { useEffect, useState } from "react";

const RequireAuth = () => {
    const [auth, setAuth] = useAuth();
    const addNotification = useNotification().addNotification;
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!auth || !auth.token) {
            navigate('/login');
            setLoading(false);
            return;
        }

        const currentDate = new Date();
        let tokenExpiresDate = new Date(auth.tokenExpiresDate);
        debugger
        if (tokenExpiresDate < currentDate) {
            setAuth();
            const notification = { color: Color.error, id: new Date().getTime(), text: 'Poświadczenie wygasło. Zaloguj się ponownie', timeToClose: 5000 };
            addNotification(notification);
            navigate('/login');
            setLoading(false);
            return;
        }

        setLoading(false);
    }, [auth, setAuth, addNotification, navigate]);

    if (loading) {
        return <p>Loading...</p>;
    }

    return <Outlet />;
}

export default RequireAuth