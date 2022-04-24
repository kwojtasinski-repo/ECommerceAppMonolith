import ItemForm from "../ItemForm";

function AddItem(props) {
    return (
        <div>
            <ItemForm cancelEditUrl = "/items"
                      cancelButtonText = "Anuluj"
                      buttonText = "Dodaj" />
        </div>
    )
}

export default AddItem;