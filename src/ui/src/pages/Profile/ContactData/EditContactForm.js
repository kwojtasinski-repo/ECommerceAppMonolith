import { useCallback, useEffect, useState } from "react";
import { useNavigate, useOutletContext } from "react-router";
import axios from "../../../axios-setup";
import { mapToCustomer } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import ContactForm from "./ContactForm";
import { requestPath } from "../../../constants";

function EditContactForm(props) {
    const [id, setId] = useState(props.id);
    const [contact, setContact] = useState(null);
    const navigate = useNavigate();
    const { addAction } = useOutletContext();
    const [error, setError] = useState('');

    const fetchCustomer = useCallback(async () => {
        try {
            const response = await axios.get(requestPath.contactsModule.getCustomer(id));
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
    }, [id])
    
    const submit = async (form) => {
        try {
            await Promise.all([
                axios.put(requestPath.contactsModule.updateAddress(form.customer.id), form.customer),
                axios.put(requestPath.contactsModule.updateAddress(form.address.id), form.address)
            ]);
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
        setId(props.id);
    }, [props.id])

    useEffect(() => {
        fetchCustomer();
    }, [fetchCustomer]);

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
            {contact ?
                <div className="card-body">
                    <ContactForm
                        contact = {contact}
                        buttonText = "Zatwierdź"
                        onSubmit = {submit}
                        cancelEditUrl = {props.cancelEditUrl}
                        cancelButtonText = {props.cancelButtonText} />
                </div>
            : null}
        </div>
    )
}

export default EditContactForm;