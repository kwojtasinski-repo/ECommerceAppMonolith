import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Input from "../../../components/Input/Input";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import { mapToMessage, validate } from "../../../helpers/validation";
import useAuth from "../../../hooks/useAuth";
import axios from '../../../axios-setup';

function Register() {
    const navigate = useNavigate();
    const [auth] = useAuth();
    const [loading, setLoading] = useState(false);
    const [form, setForm] = useState({
        email: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', 'email']
        },
        password: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', 'password']
        }
    });
    const [error, setError] = useState('');
    const valid = !Object.values(form)
                        .map(input => input.error)
                        .filter(error => error)
                        .length;

    const submit = async event => {
        event.preventDefault();
        const errorsEmail = validate(form.email.rules, form.email.value);
        const errorsPassword = validate(form.password.rules, form.password.value);
        setForm({
            ...form,
            email: {
                ...form.email,
                value: form.email.value,
                showError: true,
                error: errorsEmail
            },
            password: {
                ...form.password,
                value: form.password.value,
                showError: true,
                error: errorsPassword
            }
        });

        if (errorsEmail.length || errorsPassword.length) {
            setLoading(false);
            return;
        }

        setLoading(true);

        try {
            const response = await axios.post('users-module/account/sign-up', {
                email: form.email.value,
                password: form.password.value,
                claims: {
                    permissions: []
                }
            });
            // informacja o poprawnym zarejestrowaniu sie
            console.log(response);
        } catch(exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status);
            }
            
            setError(errorMessage);
            setLoading(false);
        }
    };

    const changeHandler = (value, fieldName) => {
        const error = validate(form[fieldName].rules, value);

        setForm({
            ...form,
            [fieldName]: {
                ...form[fieldName],
                value,
                showError: true,
                error
            }
        });
    };

    useEffect(() => {
        if (auth) {
            navigate('/');
        }
    }, [auth]);

    return (
        <div className="card">
            <div className="card-header">Rejestracja</div>
            <div className="card-body">

                <p className="text-muted">Uzupełnij dane</p>

                <form onSubmit={submit}>
                    <Input
                        label = "Email"                    
                        type = "email"
                        value = {form.email.value}
                        onChange = {val => changeHandler(val, 'email')}
                        error = {form.email.error}
                        showError = {form.email.showError} />

                    <Input
                        label = "Hasło"                    
                        type = "password"
                        value = {form.password.value}
                        onChange = {val => changeHandler(val, 'password')}
                        error = {form.password.error}
                        showError = {form.password.showError} />

                    {error ? (
                        <div className="alert alert-danger">{error}</div>
                    ) : null}

                    <div className="text-end">
                        <LoadingButton
                            loading={loading} 
                            disabled={!valid} >
                                Zarejestruj
                        </LoadingButton>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default Register;