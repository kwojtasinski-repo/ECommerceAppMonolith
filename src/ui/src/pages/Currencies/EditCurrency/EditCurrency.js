import { useEffect, useState } from "react";
import { useNavigate, useOutletContext, useParams } from 'react-router-dom';
import axios from "../../../axios-setup";
import { Color } from "../../../components/Notification/Notification";
import { mapToCurrency } from "../../../helpers/mapper";
import useNotification from "../../../hooks/useNotification";
import CurrencyForm from "../CurrencyForm";

function EditCurrency(props) {
    const { id } = useParams();
    const [currency, setCurrency] = useState([]);
    const navigate = useNavigate();
    const [notifications, addNotification] = useNotification();
    const { setRefresh } = useOutletContext();

    const fetchCurrency = async () => {
        const response = await axios.get(`/currencies-module/currencies/${id}`);
        setCurrency(mapToCurrency(response.data));
    }

    const submit = async form => {
        await axios.put(`/currencies-module/currencies/${id}`, form);
        const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zaaktualizowano', timeToClose: 5000 };
        addNotification(notification);
        setRefresh(true);
        navigate('/currencies');
    }

    useEffect(() => {
        fetchCurrency();
    }, []);

    return (
        <div className="card">
            <div className="card-header">Edytuj walutę</div>
            <div className="card-body">
            
            <p className="text-muted">Uzupełnij dane waluty</p>

                <CurrencyForm 
                    currency = {currency}
                    buttonText = "Zapisz!"
                    cancelButtonText = "Anuluj"
                    onSubmit = {submit}
                    cancelEditUrl = "/currencies" />
            </div>
        </div>
    )
}

export default EditCurrency;