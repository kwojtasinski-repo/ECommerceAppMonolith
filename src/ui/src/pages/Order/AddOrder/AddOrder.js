import useAuth from "../../../hooks/useAuth";
import ContactForm from "../../Profile/ContactData/ContactForm";
import axios from "../../../axios-setup";
import AddContact from "../../Profile/ContactData/AddContact/AddContact";

function AddOrder(props) {
    const [auth] = useAuth();

    const submit = async (form) => {
    }

    return (
        <div>
            <AddContact />
        </div>
    )
}

export default AddOrder;