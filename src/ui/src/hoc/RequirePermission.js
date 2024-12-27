import { useEffect, useState } from 'react';
import { Outlet, useNavigate } from 'react-router';
import useAuth from '../hooks/useAuth';

const RequirePermission = ({ policies = [], matchAny = false }) => {
    const auth = useAuth()[0];
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        if (policies.length === 0) {
            setLoading(false);
            return;
        }

        const permissions = auth?.claims?.permissions ?? [];
        const hasPermission = matchAny
            ? policies.some(policy => permissions.includes(policy))
            : policies.every(policy => permissions.includes(policy));

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
