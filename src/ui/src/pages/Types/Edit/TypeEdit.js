import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { useNavigate, useOutletContext, useParams } from "react-router-dom";
import { Color } from "../../../components/Notification/Notification";
import { mapToType } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import useNotification from "../../../hooks/useNotification";
import TypeForm from "../TypeForm";

function TypeEdit(props) {
    const { id } = useParams();
    const [type, setType] = useState([]);
    const navigate = useNavigate();
    const [notifications, addNotification] = useNotification();
    const { addAction } = useOutletContext();
    const [error, setError] = useState('');

    const fetchType = async () => {
        try {
            const response = await axios.get(`/items-module/types/${id}`);
            setType(mapToType(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }
    }

    const submit = async form => {
        await axios.put(`/items-module/types`, form);
        const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zaaktualizowano', timeToClose: 5000 };
        addNotification(notification);
        addAction(`Updated-type-${id}`);
        navigate('/types');
    }

    useEffect(() => {
        fetchType();
    }, []);

    return (
        <div className="card">
            <div className="card-header">Edytuj walutę</div>
            <div className="card-body">
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}

            <p className="text-muted">Uzupełnij dane typu przedmiotu</p>

                <TypeForm 
                    type = {type}
                    buttonText = "Zapisz"
                    cancelButtonText = "Anuluj"
                    onSubmit = {submit}
                    cancelEditUrl = "/types" />
            </div>
        </div>
    )
}

export default TypeEdit;