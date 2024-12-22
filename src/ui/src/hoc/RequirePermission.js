import { useEffect, useState } from 'react';
import { Outlet, useNavigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const RequirePermission = ({ children, policies = [], matchAny = false }) => {
    const auth = useAuth()[0];
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        if (policies.length === 0) {
            setLoading(false);
            return;
        }
        const hasPermission = matchAny
            ? policies.some(policy => auth.claims.permissions.includes(policy))
            : policies.every(policy => auth.claims.permissions.includes(policy));

        if (!hasPermission) {
            navigate('/not-found');
            setLoading(false);
            return
        }

        setLoading(false);
    }, [auth, navigate, policies, matchAny]);

    if (loading) {
        return <p>Loading...</p>;
    }

    return <Outlet />;
};

export default RequirePermission;
