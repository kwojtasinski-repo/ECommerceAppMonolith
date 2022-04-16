import ContactForm from "./ContactForm";
import { useNavigate, useOutletContext } from "react-router-dom";
import axios from "../../../axios-setup";

function AddContactForm(props) {
    const navigate = useNavigate();
    const { addAction } = useOutletContext();


    const submit = async (form) => {
        delete form.customer.id;
        const responseCustomer = await axios.post('/contacts-module/customers', form.customer);
        const customerId = responseCustomer.headers.location.split('/contacts-module/customers/')[1];
        delete form.address.id;
        form.address.customerId = customerId;
        await axios.post('/contacts-module/addresses', form.address);
        addAction('addContact');
        navigate(props.navigateAfterSend);
    }

    return (
        <div className="card">
            <div className="card-header">
                Nowe dane osobowe
            </div>
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