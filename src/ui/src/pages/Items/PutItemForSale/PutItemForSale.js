import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink, useNavigate, useParams } from "react-router-dom";
import { mapToCurrencies, mapToItemDetails } from "../../../helpers/mapper";
import { mapToMessage, validate } from "../../../helpers/validation";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import Input from "../../../components/Input/Input";
import Tags from "../../../components/Tags/Tags";
import Gallery from "../../../components/Gallery/Gallery";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";

function PutItemForSale(props) {
    const { id } = useParams();
    const [item, setItem] = useState();
    const [currencies, setCurrencies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [loadingButton, setLoadingButton] = useState(false);
    const [error, setError] = useState('');
    const [form, setForm] = useState({
        itemId: {
            value: id
        }, 
        itemCost: {
            value: '',
            error: '',
            showError: false,
            rules: ['required']
        },
        currencyCode: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 3 }]
        }
    });
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
        
        setLoading(false);
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

    useEffect(() => {
        fetchItem();
        fetchCurrencies();        
    }, []);

    useEffect(() => {
        if (currencies && currencies.length > 0) {
            const currency = currencies.find(c => true);
            setForm({
                ...form,
                currencyCode: {
                    ...form.currencyCode,
                    value: currency.code
                }
            })
        }
    }, [currencies])

    const submit = async (event) => {
        event.preventDefault();
        setLoadingButton(true);
        await axios.post('/items-module/item-sales', {
            itemId: form.itemId.value,
            itemCost: form.itemCost.value,
            currencyCode: form.currencyCode.value
        });
        navigate('/items');
    }

    const changeHandler = (value, fieldName) => {
        const error = validate(form[fieldName].rules, value);
        setForm({
            ...form, 
            [fieldName]: {
                ...form[fieldName],
                value,
                showError: true,
                error: error
            } 
        });
    };

    return (
        <>
        {loading ? <LoadingIcon /> :
            <div>
                <h4>{props.text}</h4>
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : null}
                <form onSubmit={submit} >
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
                    <div>
                        <Input label = "Cena"
                               type = "number"
                               value = {form.itemCost.value}
                               error = {form.itemCost.error}
                               showError = {form.itemCost.showError}
                               onChange = {val => changeHandler(val, 'itemCost')} />
                        <Input label = "Waluta"
                               type = "select" 
                               value = {form.currencyCode.value}
                               error = {form.currencyCode.error}
                               showError = {form.currencyCode.showError}
                               options = { currencies.map(t => {
                                    return {
                                        key: t.id,
                                        value: t.code,
                                        label: t.code
                                    }
                                })} 
                               onChange = {val => changeHandler(val, 'currencyCode')} />
                    </div>
                    <div className="text-end mt-2">
                        <LoadingButton
                            loading={loadingButton} 
                            className="btn btn-success">
                            Wystaw
                        </LoadingButton>
                        <NavLink className="btn btn-secondary ms-2" to = '/items' >Anuluj</NavLink>
                    </div>
                </form>
            </div>
        }
        </>
    )
}

export default PutItemForSale;