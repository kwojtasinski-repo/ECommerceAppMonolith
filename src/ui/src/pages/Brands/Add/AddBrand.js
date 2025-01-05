import axios from "../../../axios-setup";
import { useNavigate, useOutletContext } from "react-router";
import { Color } from "../../../components/Notification/Notification";
import useNotification from "../../../hooks/useNotification";
import BrandForm from "../BrandForm";
import { requestPath } from "../../../constants";

function AddBrand() {
    const navigate = useNavigate();
    const addNotification = useNotification().addNotification;
    const { addAction } = useOutletContext();

    const submit = async form => {
        await axios.post(requestPath.itemsModule.addBrand, form);
        const notification = { color: Color.success, id: new Date().getTime(), text: `Pomyślnie dodano firmę ${form.name}`, timeToClose: 5000 };
        addNotification(notification);
        addAction('added-new-brand');
        navigate('/brands');
    }

    return (
        <div className="card">
            <div className="card-header">Dodaj firmę</div>
            <div className="card-body">
            
            <p className="text-muted">Uzupełnij dane firmy</p>

                <BrandForm 
                    buttonText="Zapisz"
                    cancelButtonText = "Anuluj"
                    onSubmit = {submit}
                    cancelEditUrl = "/brands" />
            </div>
        </div>
    )
}

export default AddBrand;