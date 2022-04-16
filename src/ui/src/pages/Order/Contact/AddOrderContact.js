import AddContactForm from "../../Profile/ContactData/AddContactForm";

function AddOrderContact(props) {
    return (
        <div className="card">
            <AddContactForm
                navigateAfterSend = "/orders/add"
                cancelEditUrl = "/orders/add"
                cancelButtonText = "Anuluj"/>
        </div>
    )
}

export default AddOrderContact;