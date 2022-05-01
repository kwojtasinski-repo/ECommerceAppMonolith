import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import Input from "../../components/Input/Input";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { mapToMessage, validate } from "../../helpers/validation";

function BrandForm(props) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const [form, setForm] = useState({
        id: {
            value: ''
        },
        name: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        }
    });

    const submit = async e => {
        e.preventDefault();
        setLoading(true);
        
        try {
            await props.onSubmit({
                brandId: form.id.value,
                name: form.name.value
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
        for (const key in props.brand) {
            newForm[key].value = props.brand[key];
        }

        setForm(newForm);
    }, [props.brand]);

    return (
        <>
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            <form onSubmit={submit} >
                <Input label = "Nazwa"
                       type = "text"
                       value = {form.name.value}
                       error = {form.name.error}
                       showError = {form.name.showError}
                       onChange = {val => changeHandler(val, 'name')} />

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

export default BrandForm;