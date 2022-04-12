import { useState } from "react";
import Input from "../../components/Input/Input";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { validate } from "../../helpers/validation";

function OrderForm(props) {
    const [loading, setLoading] = useState(false);
    const [form, setForm] = useState({
        firstName: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        lastName: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        company: {
            value: '',
            error: '',
            showError: false,
            rules: ['optional', { rule: 'min', length: 3 }]
        },
        nip: {
            value: '',
            error: '',
            showError: false,
            rules: ['optional', { rule: 'only', length: 10 }]
        },
        phoneNumber: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 9 }]
        },
        country: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        zipCode: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        city: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        street: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        buildingNumber: {
            value: '',
            error: '',
            showError: false,
            rules: ['required']
        },
        localeNumber: {
            value: undefined,
            error: '',
            showError: false,
            rules: []
        }
    });

    const changeHandler = (value, fieldName) => {
        const error = validate(value[fieldName].rule, value);
        setForm({...form, 
                [fieldName]: {
                    ...form[fieldName],
                    value,
                    showError: true,
                    error
                }});
    }

    const submit = async (event) => {
        event.preventDefault();
        console.log('clicked');
    }

    return (
        <form onSubmit={submit}>
            <div className="row">
                <div className="col">
                    <Input
                        label="Imię"
                        value={form.firstName.value}
                        onChange={val => changeHandler(val, 'firstName')}
                        error={form.firstName.error}
                        showError={form.firstName.showError} />
                </div>
                <div className="col">
                    <Input
                        label="Nazwisko"
                        value={form.lastName.value}
                        onChange={val => changeHandler(val, 'lastName')}
                        error={form.lastName.error}
                        showError={form.lastName.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Input
                        label="Nazwa firma"
                        value={form.company.value}
                        onChange={val => changeHandler(val, 'company')}
                        error={form.company.error}
                        showError={form.company.showError} />
                </div>
                <div className="col">
                    <Input
                        label="NIP"
                        value={form.nip.value}
                        onChange={val => changeHandler(val, 'nip')}
                        error={form.nip.error}
                        showError={form.nip.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Input
                        label="Kraj"
                        value={form.country.value}
                        onChange={val => changeHandler(val, 'country')}
                        error={form.country.error}
                        showError={form.country.showError} />
                </div>
                <div className="col">
                    <Input
                        label="Kod pocztowy"
                        value={form.zipCode.value}
                        onChange={val => changeHandler(val, 'zipCode')}
                        error={form.zipCode.error}
                        showError={form.zipCode.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Input
                        label="Miasto"
                        value={form.city.value}
                        onChange={val => changeHandler(val, 'city')}
                        error={form.city.error}
                        showError={form.city.showError} />
                </div>
                <div className="col">
                    <Input
                        label="Ulica"
                        value={form.street.value}
                        onChange={val => changeHandler(val, 'street')}
                        error={form.street.error}
                        showError={form.street.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col-md-3">
                    <Input
                        label="Nr budynku"
                        value={form.buildingNumber.value}
                        onChange={val => changeHandler(val, 'buildingNumber')}
                        error={form.buildingNumber.error}
                        showError={form.buildingNumber.showError} />
                </div>
                <div className="col-md-3">
                    <Input
                        label="Nr lokalu"
                        type="number"
                        value={form.localeNumber.value}
                        onChange={val => changeHandler(val, 'localeNumber')}
                        error={form.localeNumber.error}
                        showError={form.localeNumber.showError} />
                </div>
                <div className="col-md-5">
                    <Input
                        label="Numer telefonu"
                        value={form.phoneNumber.value}
                        onChange={val => changeHandler(val, 'phoneNumber')}
                        error={form.phoneNumber.error}
                        showError={form.phoneNumber.showError} />
                </div>
                <div className = "text-end mt-4">
                    <LoadingButton 
                        loading = {loading} 
                        className = "btn btn-success">
                            {props.buttonText}
                    </LoadingButton>
                </div>
            </div>
        </form>
    )
}

export default OrderForm;