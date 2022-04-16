import ContactForm from "./ContactForm";
import { useNavigate } from "react-router-dom";
import axios from "../../../axios-setup";

function AddContactForm(props) {
    const navigate = useNavigate();

    const submit = async (form) => {
        const responseCustomer = await axios.post('/contacts-module/customers', form.customer);
        const customerId = responseCustomer.headers.location.split('/contacts-module/customers/')[1];
        form.address.customerId = customerId;
        await axios.post('/contacts-module/addresses', form.address);
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