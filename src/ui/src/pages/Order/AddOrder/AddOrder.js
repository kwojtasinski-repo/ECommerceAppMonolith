import useAuth from "../../../hooks/useAuth";
import ContactForm from "../ContactForm";
import axios from "../../../axios-setup";

function AddOrder(props) {
    const [auth] = useAuth();

    const submit = async (form) => {
        console.log(form);
        debugger;
        const responseCustomer = await axios.post('/contacts-module/customers', form.customer);
        const customerId = responseCustomer.headers.location.split('/contacts-module/customers/')[1];
        form.address.customerId = customerId;
        const responseAddress = await axios.post('/contacts-module/addresses', form.address);
        //location: "http://localhost:5010/contacts-module/customers/b9502e72-254b-4f4c-acd7-e37fcba7b11c"
    }

    return (
        <div className="card">
            <div className="card-header">
                Nowe zamówienie
            </div>
            <div className="card-body">
                <ContactForm
                    buttonText = "Utwórz zamówienie"
                    onSubmit = {submit} />
            </div>
        </div>
    )
}

export default AddOrder;