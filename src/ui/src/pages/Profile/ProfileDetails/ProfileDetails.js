import { useEffect, useState } from "react";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import { mapToMessage, validateEmail, validatePassword } from "../../../helpers/validation";
import useAuth from "../../../hooks/useAuth";
import axios from "../../../axios-setup";
import useNotification from "../../../hooks/useNotification";
import { Color } from "../../../components/Notification/Notification";

function ProfileDetails() {
    const [auth, setAuth] = useAuth();
    const [email, setEmail] = useState(auth?.email ?? '');
    const [password, setPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [newPasswordConfirm, setNewPasswordConfirm] = useState('');
    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState({
        email: '',
        password: '',
        newPassword: '',
        newPasswordConfirm: '',
    });
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const addNotification = useNotification().addNotification;

    const buttonDisabled = Object.values(errors).filter(x => x).length;

    const submit = async (event) => {
        event.preventDefault();
        setLoading(true);
        
        try {
            const data = {
                oldEmail: auth.email,
                newEmail: email,
                oldPassword: password,
                newPassword: newPassword,
                newPasswordConfirm: newPasswordConfirm
            };

            if (password) {
                data.password = password;
            }

            const response = await axios.post('users-module/account/change-credentials', data);

            if (response.data) {
                setAuth({
                    email: response.data.email,
                    token: response.data.accessToken,
                    userId: response.data.id,
                    claims: response.data.claims
                });
                const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zmieniono dane logowania', timeToClose: 5000 };
                addNotification(notification);
                setSuccess(true);
                setPassword('');
                setNewPassword('');
                setNewPasswordConfirm('');
            }            
        } catch (exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errorsFromApi = exception.response.data.errors;
            
            for(const errMsg in errorsFromApi) {
                errorMessage += mapToMessage(errorsFromApi[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
            setLoading(false);
        }

        setLoading(false);
    };
    
    useEffect(() => {
        if (validateEmail(email)) {
            setErrors(prevErrors => ({...prevErrors, email: ""}));
        } else {
            setErrors(prevErrors => ({...prevErrors, email: "Niepoprawny email"}));
        }

    }, [email])

    useEffect(() => {
        if (!newPassword && !newPasswordConfirm) {
            return;
        }

        let validNewPassword = '';
        let validNewPasswordConfirm = '';
        const passwordNotSame ='Hasła nie są identyczne';

        if (newPassword) {
            if (!validatePassword(newPassword)) {
                validNewPassword = 'Hasło powinno zawierać przynajmniej 8 znaków, w tym jedną dużą literę i jedną liczbę';
            }
        }

        if (newPasswordConfirm) {
            if (!validatePassword(newPasswordConfirm)) {
                validNewPasswordConfirm = 'Hasło powinno zawierać przynajmniej 8 znaków, w tym jedną dużą literę i jedną liczbę';
            }
        }

        if (!validNewPassword && !validNewPasswordConfirm) {
            if (newPassword !== newPasswordConfirm) {
                validNewPassword = passwordNotSame;
                validNewPasswordConfirm = passwordNotSame;
            }
        }

        setErrors(prevErrors => ({...prevErrors, newPassword: validNewPassword,  newPasswordConfirm: validNewPasswordConfirm}));
    }, [newPassword, newPasswordConfirm])

    return (
        <>
        <form onSubmit={submit}>
            {success ? (
                <div className="alert alert-success">Dane zapisane</div>
            ) : null}

                <div className="form-group">
                    <label>Email</label>
                    <input name="email" 
                           value={email} 
                           onChange={event => setEmail(event.target.value)} 
                           type="email" 
                           className={`form-control ${errors.email ? 'is-invalid' : 'is-valid'}`} />
                    <div className="invalid-feedback">
                        {errors.email}
                    </div>
                </div>
                <div className="form-group">
                    <label>Hasło</label>
                    <input name="password" 
                           onChange={event => setPassword(event.target.value)} 
                           type="password" 
                           className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                           autoComplete="off" />
                    <div className="invalid-feedback">
                        {errors.password}
                    </div>
                </div>
                <div className="form-group">
                    <label>Nowe hasło</label>
                    <input name="password" 
                           onChange={event => setNewPassword(event.target.value)} 
                           type="password" 
                           className={`form-control ${errors.newPassword ? 'is-invalid' : ''}`}
                           autoComplete="off" />
                    <div className="invalid-feedback">
                        {errors.newPassword}
                    </div>
                </div>
                <div className="form-group mb-2">
                    <label>Nowe hasło</label>
                    <input name="password" 
                           onChange={event => setNewPasswordConfirm(event.target.value)} 
                           type="password" 
                           className={`form-control ${errors.newPasswordConfirm ? 'is-invalid' : ''}`}
                           autoComplete="off" />
                    <div className="invalid-feedback">
                        {errors.newPasswordConfirm}
                    </div>
                </div>
                
                {error ? (
                    <div className="alert alert-danger mb-2">{error}</div>
                ) : null}

                <LoadingButton loading={loading}
                        disabled={buttonDisabled} >Zapisz</LoadingButton>
        </form>
        </>
    )
}

export default ProfileDetails;