import { useState } from "react";
import Input from "../../components/Input/Input";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { validate } from "../../helpers/validation";

function OrderForm(props) {
    const [loading, setLoading] = useState(false);
    const [isCompany, setIsCompany] = useState(props.company ? props.company : false);
    const [customerForm, setCustomerForm] = useState({
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
            value: false,
            error: '',
            showError: false,
            rules: []
        },
        companyName: {
            value: null,
            error: '',
            showError: false,
            rules: [{ rule: 'requiredIf', isRequired: isCompany, rules: [{ rule: 'min', length: 3 }] }]
        },
        nip: {
            value: null,
            error: '',
            showError: false,
            rules: [{ rule: 'requiredIf', isRequired: isCompany, rules: [{ rule: 'only', length: 10 }] }]
        },
        phoneNumber: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 9 }]
        }
    });

    const [addressForm, setAddressForm] = useState({
        countryName: {
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
        cityName: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        streetName: {
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

    const changeHandlerCustomer = (value, fieldName) => {
        const error = validate(customerForm[fieldName].rules, value);
        setCustomerForm({...customerForm, 
                [fieldName]: {
                    ...customerForm[fieldName],
                    value,
                    showError: true,
                    error
                }});
    }

    const changeHandlerAddress = (value, fieldName) => {
        const error = validate(addressForm[fieldName].rules, value);
        setAddressForm({...addressForm, 
                [fieldName]: {
                    ...addressForm[fieldName],
                    value,
                    showError: true,
                    error
                }});
    }

    const submit = async (event) => {
        event.preventDefault();
        setLoading(true);
        debugger;
        for(let field in customerForm) {
            const error = validate(customerForm[field].rules, customerForm[field].value);

            if (error) {
                setCustomerForm({...customerForm, 
                    [field]: {
                        ...customerForm[field],
                        showError: true,
                        error
                    }});
                setLoading(false);
                return;
            }
        }
        
        for(let field in addressForm) {
            const error = validate(addressForm[field].rules, addressForm[field].value);

            if (error) {
                setAddressForm({...addressForm, 
                    [field]: {
                        ...addressForm[field],
                        showError: true,
                        error
                    }});
                setLoading(false);
                return;
            }
        }

        console.log('clicked');
        const newForm = {customer: {}, address: {}};
        for (const key in customerForm) {
            newForm.customer[key] = customerForm[key].value;
        }
        for (const key in addressForm) {
            newForm.address[key] = addressForm[key].value;
        }
        debugger;
        props.onSubmit(newForm);
        setLoading(false);
    }

    const companyHandler = (value) => {
        console.log(value);
        setIsCompany(value);
        let rulesCompany = customerForm.companyName.rules;
        rulesCompany[0].isRequired = value;
        let rulesNip = customerForm.nip.rules;
        rulesNip[0].isRequired = value;
        setCustomerForm({
            ...customerForm,
                companyName: {
                    ...customerForm.companyName,
                    rules: rulesCompany
                },
                rules: {
                    ...customerForm.nip,
                    rules: rulesNip
                }
        });
    }

    return (
        <form onSubmit={submit}>
            <div className="row">
                <div className="col card-header"  
                    style={isCompany ? null : {background: "#6fcbde"}}
                    onClick={ () => companyHandler(false)}>
                    Osoba prywatna
                </div>
                <div className="col card-header" 
                    style={isCompany ? {background: "#6fcbde"} : null}
                    onClick={ () => companyHandler(true) }>
                    Firma
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Input
                        label="Imię"
                        value={customerForm.firstName.value}
                        onChange={val => changeHandlerCustomer(val, 'firstName')}
                        error={customerForm.firstName.error}
                        showError={customerForm.firstName.showError} />
                </div>
                <div className="col">
                    <Input
                        label="Nazwisko"
                        value={customerForm.lastName.value}
                        onChange={val => changeHandlerCustomer(val, 'lastName')}
                        error={customerForm.lastName.error}
                        showError={customerForm.lastName.showError} />
                </div>
            </div>
            <div className="row" style={isCompany ? { display: "block" } : {display: "none"}}>
                <div className="col">
                    <Input
                        label="Nazwa firma"
                        value={customerForm.companyName.value}
                        onChange={val => changeHandlerCustomer(val, 'companyName')}
                        error={customerForm.companyName.error}
                        showError={customerForm.companyName.showError} />
                </div>
                <div className="col" style={isCompany ? { display: "block" } : {display: "none"}} >
                    <Input
                        label="NIP"
                        value={customerForm.nip.value}
                        onChange={val => changeHandlerCustomer(val, 'nip')}
                        error={customerForm.nip.error}
                        showError={customerForm.nip.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Input
                        label="Kraj"
                        value={addressForm.countryName.value}
                        onChange={val => changeHandlerAddress(val, 'countryName')}
                        error={addressForm.countryName.error}
                        showError={addressForm.countryName.showError} />
                </div>
                <div className="col">
                    <Input
                        label="Kod pocztowy"
                        type="zipCode"
                        value={addressForm.zipCode.value}
                        onChange={val => changeHandlerAddress(val, 'zipCode')}
                        error={addressForm.zipCode.error}
                        showError={addressForm.zipCode.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Input
                        label="Miasto"
                        value={addressForm.cityName.value}
                        onChange={val => changeHandlerAddress(val, 'cityName')}
                        error={addressForm.cityName.error}
                        showError={addressForm.cityName.showError} />
                </div>
                <div className="col">
                    <Input
                        label="Ulica"
                        value={addressForm.streetName.value}
                        onChange={val => changeHandlerAddress(val, 'streetName')}
                        error={addressForm.streetName.error}
                        showError={addressForm.streetName.showError} />
                </div>
            </div>
            <div className="row">
                <div className="col-md-3">
                    <Input
                        label="Nr budynku"
                        value={addressForm.buildingNumber.value}
                        onChange={val => changeHandlerAddress(val, 'buildingNumber')}
                        error={addressForm.buildingNumber.error}
                        showError={addressForm.buildingNumber.showError} />
                </div>
                <div className="col-md-3">
                    <Input
                        label="Nr lokalu"
                        type="number"
                        value={addressForm.localeNumber.value}
                        onChange={val => changeHandlerAddress(val, 'localeNumber')}
                        error={addressForm.localeNumber.error}
                        showError={addressForm.localeNumber.showError} />
                </div>
                <div className="col-md-5">
                    <Input
                        label="Numer telefonu"
                        value={customerForm.phoneNumber.value}
                        onChange={val => changeHandlerCustomer(val, 'phoneNumber')}
                        error={customerForm.phoneNumber.error}
                        showError={customerForm.phoneNumber.showError} />
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