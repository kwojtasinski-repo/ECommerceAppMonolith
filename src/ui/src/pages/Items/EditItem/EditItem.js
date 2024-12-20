import axios from "../../../axios-setup";
import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { mapToBrands, mapToItemDetails, mapToTypes } from "../../../helpers/mapper";
import ItemForm from "../ItemForm";
import { mapToMessage } from "../../../helpers/validation";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";

function EditItem() {
    const [brands, setBrands] = useState([]);
    const [types, setTypes] = useState([]);
    const { id } = useParams();
    const navigate = useNavigate();
    const [error, setError] = useState();
    const [item, setItem] = useState(null);
    const [loading, setLoading] = useState(true);

    const onSubmit = async (form) => {
        await axios.put(`/items-module/items/${id}`, form);
    }

    const redirectAfterSuccess = () => {
        navigate(`/items/details/${id}`);
    }

    const fetchBrands = async () => {
        try {
            const response = await axios.get('/items-module/brands');
            setBrands(mapToBrands(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }
        setLoading(false);
    }

    const fetchTypes = async () => {
        try {
            const response = await axios.get('/items-module/types');
            setTypes(mapToTypes(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }
        setLoading(false);
    }

    const fetchItem = useCallback(async () => {
        try {
            const response = await axios.get(`/items-module/items/${id}`);
            const itemLocal = mapToItemDetails(response.data);
            setItem(itemLocal);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response?.status;
            const errors = exception.response?.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';            
            setError(errorMessage);
        }
        
        setLoading(false);
    }, [id]);

    useEffect(() => {
        fetchBrands();
        fetchTypes();
        fetchItem();
    }, [fetchItem]);

    return (
        <>
        {loading ? <LoadingIcon /> : 
            <>
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : 
                    <div>
                        <ItemForm text = "Edytuj przedmiot"
                                  item = {item}
                                  onSubmit = {onSubmit}
                                  cancelEditUrl = "/items"
                                  cancelButtonText = "Anuluj"
                                  buttonText = "Zatwierdź"
                                  brands = {brands}
                                  types = {types}
                                  redirectAfterSuccess = {redirectAfterSuccess} />
                    </div>}
            </>}
        </>
    )
}

export default EditItem;