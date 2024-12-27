import { useParams } from "react-router";
import EditContactForm from "../../Profile/ContactData/EditContactForm";

function EditOrderContact() {
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