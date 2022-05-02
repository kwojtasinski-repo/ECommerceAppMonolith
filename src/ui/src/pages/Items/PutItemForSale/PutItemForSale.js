import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { mapToCurrencies, mapToItemDetails } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import Tags from "../../../components/Tags/Tags";
import Gallery from "../../../components/Gallery/Gallery";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import ItemSaleForm from "../ItemSaleForm";

function PutItemForSale(props) {
    const { id } = useParams();
    const [item, setItem] = useState();
    const [currencies, setCurrencies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const fetchItem = async () => {
        try {
            const response = await axios.get(`/items-module/items/${id}`);
            const itemLocal = mapToItemDetails(response.data);
            setItem(itemLocal);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response?.status;
            const errors = exception.response?.data.errors;
            errorMessage += mapToMessage(errors, status);            
            setError(errorMessage);
        }
    }

    const fetchCurrencies = async () => {
        try {
            const response = await axios.get(`/currencies-module/currencies`);
            const currenciesLocal = mapToCurrencies(response.data);
            setCurrencies(currenciesLocal);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response?.status;
            const errors = exception.response?.data.errors;
            errorMessage += mapToMessage(errors, status);            
            setError(errorMessage);
        }        
    }

    const fetchData = async () => {
        await fetchItem();
        await fetchCurrencies();
    }

    useEffect(() => {
        fetchData();
    }, []);

    useEffect(() => {
        if (item && currencies.length > 0) {
            setLoading(false);
        }
    }, [item, currencies]);

    const submit = async (form) => {
        debugger;
        await axios.post('/items-module/item-sales', form);
        navigate('/items');
    }

    return (
        <>
        {loading ? <LoadingIcon /> :
            <div>
                <h4>Wystaw przedmiot</h4>
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : null}
                <div className="form-group">
                    <label>Nazwa przedmiotu</label>
                    <input type = "text"
                            value = {item.name}
                            className = "form-control"
                            readOnly />
                </div>
                <div className="form-group">
                    <label>Opis</label>
                    <input type = "textarea"
                            value = {item.description}
                            className = "form-control"
                            readOnly />
                </div>    
                <div className="form-group">
                    <label>Firma</label>
                    <input type = "text" 
                            value = {item.brand}
                            className = "form-control"
                            readOnly />
                </div>
                <div className="form-group">
                    <label>Typ</label>
                    <input type = "text"
                            value = {item.type}
                            className = "form-control"
                            readOnly />
                </div>
                <Tags tags = {item.tags} canEdit = {false} />
                <Gallery items = {item.imagesUrl.map(i => i.url)} />
                <ItemSaleForm itemSale ={{
                                id: '',
                                itemId: id,
                                cost: '',
                                code: ''
                              }}
                              onSubmit = {submit}
                              currencies = {currencies}
                              textSubmit = "Wystaw"
                              textCancel = "Anuluj" />
            </div>
        }
        </>
    )
}

export default PutItemForSale;