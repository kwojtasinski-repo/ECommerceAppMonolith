import { useEffect, useState } from "react";
import { mapToMessage, validate } from "../../helpers/validation";
import Input from "../../components/Input/Input";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { NavLink } from "react-router-dom";

function CurrencyForm(props) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const [form, setForm] = useState({
        id: {
            value: ''
        },
        code: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 3 }]
        },
        description: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 10 }]
        }
    });

    const submit = async e => {
        e.preventDefault();
        setLoading(true);
        
        try {
            props.onSubmit({
                code: form.code.value,
                description: form.description.value
            });
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status);
            }
            
            setError(errorMessage);
        }

        setLoading(false);
    };

    const changeHandler = (value, fieldName) => {
        const error = validate(form[fieldName].rules, value);
        setForm({
            ...form, 
            [fieldName]: {
                ...form[fieldName],
                value,
                showError: true,
                error: error
            } 
        });
    };

    useEffect(() => {
        const newForm = {...form};
        for (const key in props.currency) {
            newForm[key].value = props.currency[key];
        }

        setForm(newForm);
    }, [props.currency]);

    return (
        <>
        {error ? (
            <div className="alert alert-danger">{error}</div>
        ) : null}
        <form onSubmit={submit} >
            <Input label = "Kod waluty"
                    type = "text"
                    value = {form.code.value}
                    error = {form.code.error}
                    showError = {form.code.showError}
                    onChange = {val => changeHandler(val, 'code')} />

            <Input label = "Opis"
                    type = "textarea"
                    value = {form.description.value}
                    error = {form.description.error}
                    showError = {form.description.showError}
                    onChange = {val => changeHandler(val, 'description')} />

            <div className="text-end mt-2">
                <NavLink className="btn btn-secondary me-2" to = {props.cancelEditUrl} >{props.cancelButtonText}</NavLink>
                <LoadingButton
                    loading={loading} 
                    className="btn btn-success">
                    {props.buttonText}
                </LoadingButton>
            </div>
        </form>
        </>
    )
}

export default CurrencyForm;