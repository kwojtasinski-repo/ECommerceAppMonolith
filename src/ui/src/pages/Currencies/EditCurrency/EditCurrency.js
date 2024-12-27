import { useCallback, useEffect, useState } from "react";
import { useNavigate, useOutletContext, useParams } from 'react-router';
import axios from "../../../axios-setup";
import { Color } from "../../../components/Notification/Notification";
import { mapToCurrency } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import useNotification from "../../../hooks/useNotification";
import CurrencyForm from "../CurrencyForm";

function EditCurrency() {
    const { id } = useParams();
    const [currency, setCurrency] = useState(null);
    const navigate = useNavigate();
    const addNotification = useNotification().addNotification;
    const { setRefresh } = useOutletContext();
    const [error, setError] = useState('');

    const fetchCurrency = useCallback(async () => {
        try {
            const response = await axios.get(`/currencies-module/currencies/${id}`);
            setCurrency(mapToCurrency(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }
    }, [id])

    const submit = async form => {
        await axios.put(`/currencies-module/currencies/${id}`, form);
        const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zaaktualizowano', timeToClose: 5000 };
        addNotification(notification);
        setRefresh(true);
        navigate('/currencies');
    }

    useEffect(() => {
        fetchCurrency();
    }, [fetchCurrency]);

    return (
        <div className="card">
            <div className="card-header">Edytuj walutę</div>
            <div className="card-body">
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}

            {currency ?
            <>
                <p className="text-muted">Uzupełnij dane waluty</p>

                    <CurrencyForm 
                        currency = {currency}
                        buttonText = "Zapisz!"
                        cancelButtonText = "Anuluj"
                        onSubmit = {submit}
                        cancelEditUrl = "/currencies" />
            </>
            : null}
            </div>
        </div>
    )
}

export default EditCurrency;