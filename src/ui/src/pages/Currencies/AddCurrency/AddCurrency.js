import CurrencyForm from "../CurrencyForm";
import axios from "../../../axios-setup";
import { useNavigate, useOutletContext } from "react-router";
import useNotification from "../../../hooks/useNotification";
import { Color } from "../../../components/Notification/Notification";

function AddCurrency() {
    const navigate = useNavigate();
    const addNotification = useNotification().addNotification;
    const { setRefresh } = useOutletContext();

    const submit = async form => {
        await axios.post("/currencies-module/currencies", form);
        const notification = { color: Color.success, id: new Date().getTime(), text: `Pomyślnie dodano walutę ${form.code}`, timeToClose: 5000 };
        addNotification(notification);
        setRefresh(true);
        navigate('/currencies');
    }

    return (
        <div className="card">
            <div className="card-header">Dodaj walutę</div>
            <div className="card-body">
            
            <p className="text-muted">Uzupełnij dane waluty</p>

                <CurrencyForm 
                    buttonText="Zapisz!"
                    cancelButtonText = "Anuluj"
                    onSubmit = {submit}
                    cancelEditUrl = "/currencies" />
            </div>
        </div>
    )
}

export default AddCurrency;