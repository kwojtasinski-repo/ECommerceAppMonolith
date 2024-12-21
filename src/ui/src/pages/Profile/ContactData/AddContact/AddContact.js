import AddContactForm from "../AddContactForm";

function AddContact() {
    return (
        <div className="card">
            <AddContactForm
                navigateAfterSend = "/profile/contact-data"
                cancelEditUrl = "/profile/contact-data"
                cancelButtonText = "Anuluj"/>
        </div>
    )
}

export default AddContact;