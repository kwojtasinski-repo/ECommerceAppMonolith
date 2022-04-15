import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "../../../../axios-setup";
import { mapToCustomer } from "../../../../helpers/mapper";
import ContactForm from "../ContactForm";

function EditContact(props) {
    const { id } = useParams();
    const [contact, setContact] = useState(null);
    const navigate = useNavigate();

    const fetchCustomer = async () => {
        debugger;
        const response = await axios.get(`/contacts-module/customers/${id}`);
        let customer = mapToCustomer(response.data);
        const address = {...customer.address};
        delete customer.address;
        setContact({ customer, address });
    }

    const submit = async (form) => {
        await axios.put('/contacts-module/customers', form.customer);
        await axios.put('/contacts-module/addresses', form.address);
        navigate('/profile/contact-data');
    }

    useEffect(() => {
        fetchCustomer();
    }, []);

    return (
        <div className="card">
            <div className="card-header">
                Edytuj dane osobowe
            </div>
            <div className="card-body">
                <ContactForm
                    contact = {contact}
                    buttonText = "Zatwierdź"
                    onSubmit = {submit}
                    cancelEditUrl = "/profile/contact-data"
                    cancelButtonText = "Anuluj" />
            </div>
        </div>
    )
}

export default EditContact;