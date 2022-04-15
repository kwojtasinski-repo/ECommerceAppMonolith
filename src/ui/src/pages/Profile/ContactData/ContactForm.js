import { useState } from "react";
import Input from "../../../components/Input/Input";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import { isEmpty } from "../../../helpers/stringExtensions";
import { validate } from "../../../helpers/validation";

function ContactForm(props) {
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
            value: undefined,
            error: '',
            showError: false,
            rules: [{ rule: 'requiredIf', isRequired: isCompany, rules: [{ rule: 'min', length: 3 }] }]
        },
        nip: {
            value: undefined,
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
        changeHandler(value, fieldName, customerForm, setCustomerForm);
    }

    const changeHandlerAddress = (value, fieldName) => {
        changeHandler(value, fieldName, addressForm, setAddressForm);
    }

    const changeHandler = (value, fieldName, form, setForm) => {
        const error = validate(form[fieldName].rules, value);
        setForm({...form, 
                [fieldName]: {
                    ...form[fieldName],
                    value,
                    showError: true,
                    error
                }});
    }

    const validateBeforeSend = (form, setForm) => {
        for(let field in form) {
            const error = validate(form[field].rules, form[field].value);

            if (error) {
                setForm({...form, 
                    [field]: {
                        ...form[field],
                        showError: true,
                        error
                    }});
                setLoading(false);
                return error;
            }
        }
    }

    const submit = async (event) => {
        event.preventDefault();
        setLoading(true);
        const errorCustomer = validateBeforeSend(customerForm, setCustomerForm);
        const errorAddress = validateBeforeSend(addressForm, setAddressForm);

        if (!isEmpty(errorCustomer) || !isEmpty(errorAddress)) {
            setLoading(false);
            return;
        }
        
        const newForm = {customer: {}, address: {}};
        for (const key in customerForm) {
            newForm.customer[key] = customerForm[key].value;
        }
        for (const key in addressForm) {
            newForm.address[key] = addressForm[key].value;
        }
        props.onSubmit(newForm);
        setLoading(false);
    }

    const companyHandler = (value) => {
        setIsCompany(value);
        const rulesCompany = setIsRequiredRule(value, 'companyName', customerForm);
        const rulesNip = setIsRequiredRule(value, 'nip', customerForm);
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

    const setIsRequiredRule = (value, fieldName, form) => {
        let rules = form[fieldName].rules;

        for (const key in rules) {
            let rule = rules[key];
            if (rule instanceof Object) {
                if (rule.isRequired !== undefined) { // property exists in object
                    rule.isRequired = value;
                }
                return rule;
            }
        }
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

export default ContactForm;