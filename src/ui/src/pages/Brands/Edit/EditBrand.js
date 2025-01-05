import axios from "../../../axios-setup";
import { useCallback, useEffect, useState } from "react";
import { useNavigate, useOutletContext, useParams } from "react-router";
import { mapToBrand } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import useNotification from "../../../hooks/useNotification";
import { Color } from "../../../components/Notification/Notification";
import BrandForm from "../BrandForm";
import { requestPath } from "../../../constants";

function EditBrand() {
    const { id } = useParams();
    const [type, setType] = useState(null);
    const navigate = useNavigate();
    const addNotification = useNotification().addNotification;
    const { addAction } = useOutletContext();
    const [error, setError] = useState('');

    const fetchBrand = useCallback(async () => {
        try {
            const response = await axios.get(requestPath.itemsModule.getBrand(id));
            setType(mapToBrand(response.data));
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
        await axios.put(requestPath.itemsModule.updateBrand(id), form);
        const notification = { color: Color.success, id: new Date().getTime(), text: 'Pomyślnie zaaktualizowano', timeToClose: 5000 };
        addNotification(notification);
        addAction(`Updated-brand-${id}`);
        navigate('/brands');
    }

    useEffect(() => {
        fetchBrand();
    }, [fetchBrand]);

    return (
        <div className="card">
            <div className="card-header">Edytuj firmę</div>
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