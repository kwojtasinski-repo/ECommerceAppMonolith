import { Navigate } from "react-router-dom";
import { policiesAuthentication } from "../helpers/policiesAuthentication";
import useAuth from "../hooks/useAuth";

const RequireAuth = ({ children }) => {
    const [auth] = useAuth();
    const policies = policiesAuthentication(children);

    return auth && (policies.length === 0 || auth.claims?.permissions?.some(p => policies.includes(p))) ? (
        children
    ) : (
        <Navigate to="/login" />
    )
}

export default RequireAuth