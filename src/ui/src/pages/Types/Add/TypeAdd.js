import axios from "../../../axios-setup";
import { useNavigate, useOutletContext } from "react-router";
import { Color } from "../../../components/Notification/Notification";
import useNotification from "../../../hooks/useNotification";
import TypeForm from "../TypeForm";
import { requestPath } from "../../../constants";

function TypeAdd() {
    const navigate = useNavigate();
    const addNotification = useNotification().addNotification;
    const { addAction } = useOutletContext();

    const submit = async form => {
        await axios.post(requestPath.itemsModule.addType, form);
        const notification = { color: Color.success, id: new Date().getTime(), text: `Pomyślnie dodano typ ${form.name}`, timeToClose: 5000 };
        addNotification(notification);
        addAction('added-new-type');
        navigate('/types');
    }

    return (
        <div className="card">
            <div className="card-header">Dodaj typ przedmiotu</div>
            <div className="card-body">
            
            <p className="text-muted">Uzupełnij dane typu przedmiotu</p>

                <TypeForm 
                    buttonText="Zapisz"
                    cancelButtonText = "Anuluj"
                    onSubmit = {submit}
                    cancelEditUrl = "/types" />
            </div>
        </div>
    )
}

export default TypeAdd;