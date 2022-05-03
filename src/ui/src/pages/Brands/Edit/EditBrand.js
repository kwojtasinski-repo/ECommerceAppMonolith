import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { useNavigate, useOutletContext, useParams } from "react-router-dom";
import { mapToBrand } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import useNotification from "../../../hooks/useNotification";
import { Color } from "../../../components/Notification/Notification";
import BrandForm from "../BrandForm";

function EditBrand(props) {
    const { id } = useParams();
    const [type, setType] = useState(null);
    const navigate = useNavigate();
    const [notifications, addNotification] = useNotification();
    const { addAction } = useOutletContext();
    const [error, setError] = useState('');

    const fetchBrand = async () => {
        try {
            const response = await axios.get(`/items-module/brands/${id}`);
            setType(mapToBrand(response.data));
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
        await axios.put(`/items-module/brands`, form);
        const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zaaktualizowano', timeToClose: 5000 };
        addNotification(notification);
        addAction(`Updated-brand-${id}`);
        navigate('/brands');
    }

    useEffect(() => {
        fetchBrand();
    }, []);

    return (
        <div className="card">
            <div className="card-header">Edytuj walutę</div>
            <div className="card-body">
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : null}

                {type ?
                    <>
                        <p className="text-muted">Uzupełnij dane typu przedmiotu</p>

                        <BrandForm 
                            brand = {type}
                            buttonText = "Zapisz"
                            cancelButtonText = "Anuluj"
                            onSubmit = {submit}
                            cancelEditUrl = "/brands" />
                    </>
                : null}
            </div>
        </div>
    )
}

export default EditBrand;