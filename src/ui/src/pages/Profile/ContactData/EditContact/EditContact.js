import { useParams } from "react-router-dom";
import EditContactForm from "../EditContactForm";

function EditContact(props) {
    const { id } = useParams();

    return (
        <div>
            <EditContactForm
                id = {id}
                navigateAfterSend = "/profile/contact-data"
                cancelEditUrl = "/profile/contact-data"
                cancelButtonText = "Anuluj" />
        </div>
    )
}

export default EditContact;