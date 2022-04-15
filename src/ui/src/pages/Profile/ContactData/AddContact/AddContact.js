import axios from "../../../../axios-setup";
import ContactForm from "../ContactForm";

function AddContact(props) {
    const submit = async (form) => {
        const responseCustomer = await axios.post('/contacts-module/customers', form.customer);
        const customerId = responseCustomer.headers.location.split('/contacts-module/customers/')[1];
        form.address.customerId = customerId;
        await axios.post('/contacts-module/addresses', form.address);
    }

    return (
        <div className="card">
            <div className="card-header">
                Nowe dane osobowe
            </div>
            <div className="card-body">
                <ContactForm
                    buttonText = "Utwórz zamówienie"
                    onSubmit = {submit} />
            </div>
        </div>
    )
}

export default AddContact;