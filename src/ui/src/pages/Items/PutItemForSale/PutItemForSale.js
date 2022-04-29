import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { mapToItemDetails } from "../../../helpers/mapper";
import { mapToMessage, validate } from "../../../helpers/validation";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import Input from "../../../components/Input/Input";
import Tags from "../../../components/Tags/Tags";
import Gallery from "../../../components/Gallery/Gallery";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";

function PutItemForSale(props) {
    const { id } = useParams();
    const [item, setItem] = useState();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [form, setForm] = useState({
        itemId: {
            value: ''
        }, 
        itemCost: {
            value: 0,
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

    useEffect(() => {
        fetchItem();
    }, []);

    const submit = () => {

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
                    </div>
                    <div className="text-end mt-2">
                        <LoadingButton
                            loading={loading} 
                            className="btn btn-success">
                            Zatwierdź
                        </LoadingButton>
                        <NavLink className="btn btn-secondary ms-2" to = '/' >Anuluj</NavLink>
                    </div>
                </form>
            </div>
        }
        </>
    )
}

export default PutItemForSale;