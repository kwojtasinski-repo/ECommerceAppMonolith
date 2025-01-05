import axios from "../../../axios-setup";
import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { mapToCurrencies, mapToItemDetails } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import Tags from "../../../components/Tags/Tags";
import Gallery from "../../../components/Gallery/Gallery";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import ItemSaleForm from "../ItemSaleForm";
import { requestPath } from "../../../constants";

function PutItemForSale() {
    const { id } = useParams();
    const [item, setItem] = useState(null);
    const [currencies, setCurrencies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const fetchItem = useCallback(async () => {
        try {
            const response = await axios.get(requestPath.itemsModule.getItem(id));
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
    }, [id])

    const fetchCurrencies = async () => {
        try {
            const response = await axios.get(requestPath.currenciesModule.currencies);
            const currenciesLocal = mapToCurrencies(response.data);
            setCurrencies(currenciesLocal);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response?.status;
            const errors = exception.response?.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';            
            setError(errorMessage);
        }        
    }

    const fetchData = useCallback(async () => {
        await fetchItem();
        await fetchCurrencies();
    }, [fetchItem])

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    useEffect(() => {
        if (item && currencies.length > 0) {
            setLoading(false);
        }
    }, [item, currencies]);

    const submit = async (form) => {
        await axios.post(requestPath.itemsModule.addItemForSale, form);
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
                {item ?
                <>
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
                </>
                : null}
            </div>
        }
        </>
    )
}

export default PutItemForSale;