import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import useAuth from "../../../hooks/useAuth";
import axios from '../../../axios-setup';
import useNotification from "../../../hooks/useNotification";
import { Color } from "../../../components/Notification/Notification";
import { mapCodeToMessage } from "../../../helpers/errorCodeMapper";
import { getRecommendationProducts } from "../../../recommendation-products";
import { requestPath } from "../../../constants";

function Login() {
    const [auth, setAuth] = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [valid, setValid] = useState(null);
    const addNotification = useNotification().addNotification;

    const submit = async (event) => {
        event.preventDefault();
        setValid(true);
        setLoading(true);
        
        try {
            const response = await axios.post(requestPath.usersModule.login, {
                email,
                password
            });
            setAuth({
                email: response.data.email,
                token: response.data.accessToken,
                userId: response.data.id,
                claims: response.data.claims,
                tokenExpiresDate: response.data.expires
            });
            const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zalogowano', timeToClose: 5000 };
            addNotification(notification);
            await getRecommendationProducts();
            navigate('/');
        } catch (exception) {
            console.log(exception);
            setValid(false);            
            setLoading(false);
        }
    }

    useEffect(() => {
        if (auth) {
            navigate('/');
        }
    }, [auth, navigate]);

    return (
        <div>
            <h2>Logowanie</h2>
            <form onSubmit={submit}>
                <div className="form-group">
                    <label htmlFor="email-input" >Email</label>
                    <input name="email" 
                        id="email-input"
                        value={email} 
                        onChange={event => setEmail(event.target.value)} 
                        type="email" 
                        className="form-control" 
                        autoComplete="off" />
                </div>
                <div className="form-group">
                    <label htmlFor="password-input">Hasło</label>
                    <input id="password-input"
                        name="password" 
                        value={password} 
                        onChange={event => setPassword(event.target.value)} 
                        type="password" 
                        className="form-control"
                        autoComplete="off" />
                </div>

                {valid === false ? (
                    <div className="alert alert-danger mb-2 mt-2">
                        {mapCodeToMessage('invalid_credentials')}
                    </div>
                ) : null}

                <div className="mt-2">
                    <LoadingButton loading={loading} >Zaloguj</LoadingButton>
                </div>
            </form>
        </div>
    )
}

export default Login;