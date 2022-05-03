import { useEffect, useState } from "react";
import { useNavigate, useOutletContext } from "react-router-dom";
import axios from "../../../axios-setup";
import { mapToCustomer } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import ContactForm from "./ContactForm";

function EditContactForm(props) {
    const [id, setId] = useState(props.id);
    const [contact, setContact] = useState(null);
    const navigate = useNavigate();
    const { addAction } = useOutletContext();
    const [error, setError] = useState('');

    const fetchCustomer = async () => {
        try {
            const response = await axios.get(`/contacts-module/customers/${id}`);
            let customer = mapToCustomer(response.data);
            const address = {...customer.address};
            delete customer.address;
            setContact({ customer, address });
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }
    }
    
    const submit = async (form) => {
        try {
            await axios.put('/contacts-module/customers', form.customer);
            await axios.put('/contacts-module/addresses', form.address);
            addAction('editContact');
            navigate(props.navigateAfterSend);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
        }
    }

    useEffect(() => {
        fetchCustomer();
    }, []);

    return (
        <div className="card">
            <div className="card-header">
                Edytuj dane osobowe
            </div>
            {error ? (
                <div className="alert alert-danger">
                    {error}
                </div>
            ) : null}
            <div className="card-body">
                <ContactForm
                    contact = {contact}
                    buttonText = "Zatwierdź"
                    onSubmit = {submit}
                    cancelEditUrl = {props.cancelEditUrl}
                    cancelButtonText = {props.cancelButtonText} />
            </div>
        </div>
    )
}

export default EditContactForm;