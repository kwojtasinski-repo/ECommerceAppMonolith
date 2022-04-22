import ContactForm from "./ContactForm";
import { useNavigate, useOutletContext } from "react-router-dom";
import axios from "../../../axios-setup";
import { mapToMessage } from "../../../helpers/validation";
import { useState } from "react";

function AddContactForm(props) {
    const navigate = useNavigate();
    const { addAction } = useOutletContext();
    const [error, setError] = useState('');

    const submit = async (form) => {
        try {
            delete form.customer.id;
            const responseCustomer = await axios.post('/contacts-module/customers', form.customer);
            const customerId = responseCustomer.headers.location.split('/contacts-module/customers/')[1];
            delete form.address.id;
            form.address.customerId = customerId;
            await axios.post('/contacts-module/addresses', form.address);
            addAction('addContact');
            navigate(props.navigateAfterSend);
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
    }

    return (
        <div className="card">
            <div className="card-header">
                Nowe dane osobowe
            </div>
            {error ? (
                <div className="alert alert-danger">
                    {error}
                </div>
            ) : null}
            <div className="card-body">
                <ContactForm
                    buttonText = "Dodaj dane osobowe"
                    onSubmit = {submit}
                    cancelEditUrl = {props.cancelEditUrl}
                    cancelButtonText = {props.cancelButtonText} />
            </div>
        </div>
    )
}

export default AddContactForm;