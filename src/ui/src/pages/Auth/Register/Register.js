import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Input from "../../../components/Input/Input";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import { validate } from "../../../helpers/validation";
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
        setLoading(true);

        try {
            axios.post('users-module/account/sign-up', {
                email: form.email.value,
                password: form.email.value
            });
            // informacja o poprawnym zarejestrowaniu sie
        } catch(exception) {
            //setError()
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
                        type = "passowrd"
                        value = {form.email.value}
                        onChange = {val => changeHandler(val, 'password')}
                        error = {form.email.error}
                        showError = {form.email.showError} />

                    {error ? (
                        <div className="alert alert-danger">{error}</div>
                    ) : null}

                    <div className="text-end">
                        <LoadingButton
                            loading={loading} 
                            disabled={!valid} >
                                Gotowe!
                        </LoadingButton>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default Register;