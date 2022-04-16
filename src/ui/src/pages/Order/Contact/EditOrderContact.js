import { useParams } from "react-router-dom";
import EditContactForm from "../../Profile/ContactData/EditContactForm";

function EditOrderContact(props) {
    const { id } = useParams();

    return (
        <div>
            <EditContactForm
                id = {id}
                navigateAfterSend = "/orders/add"
                cancelEditUrl = "/orders/add"
                cancelButtonText = "Anuluj" />
        </div>
    )
}

export default EditOrderContact;