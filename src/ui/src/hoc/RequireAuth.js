import { Navigate } from "react-router-dom";
import { policiesAuthentication } from "../helpers/policiesAuthentication";
import useAuth from "../hooks/useAuth";
import NotFound from "../pages/404/NotFound";

const RequireAuth = ({ children }) => {
    const [auth] = useAuth();
    const policies = policiesAuthentication(children);
    let response = auth ? children : (<Navigate to="/login" />);
    
    if (auth && policies.length > 0) {
        response = auth.claims.permissions.some(p => policies.includes(p)) ? children : (<NotFound/>);
    }

    return response;
}

export default RequireAuth