import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink, useNavigate, useOutletContext } from "react-router";
import { mapToCurrencies } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import Input from "../../../components/Input/Input";
import style from "./ChangeCurrency.module.css"
import { requestPath } from "../../../constants";


function ChangeCurrency() {
    const order = useOutletContext();
    const [currencies, setCurrencies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [currency, setCurrency] = useState(order.code);
    const navigate = useNavigate();

    const fetchCurrencies = async () => {
        try {
            const response = await axios.get(requestPath.currenciesModule.currencies);
            setCurrencies(mapToCurrencies(response.data));
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

    useEffect(()=>{
        fetchCurrencies();
    }, []);

    const onChange = (value) => {
        setCurrency(value);
    }

    const submit = async (e) => {
        e.preventDefault();
        setLoading(true);
        
        try {
            await axios.patch(requestPath.salesModule.changeCurrencyOnOrder(order.orderId), {
                orderId: order.orderId, 
                currencyCode: currency
            });
            order.setAction('updatedOrder' + new Date().getTime() + order.code);
            setLoading(false);
            navigate(`/payments/add/${order.orderId}`);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
            setLoading(false);
        }
    }

    return (
        <div className="pt-2 pb-2">
        {loading ? <LoadingIcon /> : (
            <div className={style.center}>
                <div>
                    <p>Zmień walutę na zamówieniu</p>
                </div>
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : null}
                <div className={style.select}>
                    <form>
                        <div>
                        <Input type="select"
                            value = {currency}
                            options = {currencies.map(c => {
                                return { key: c.id, value: c.code, label: c.code }
                            })} 
                            onChange = {onChange} />
                        </div>
                        <div className="pt-2">
                            <button className="btn btn-success"
                                    onClick={submit}>
                                    Akceptuj
                            </button>
                            <NavLink to = {`/payments/add/${order.orderId}`} className="btn btn-secondary ms-2">Anuluj</NavLink>
                        </div>
                    </form>
                </div>
            </div>
        )}
        </div>
    )
}

export default ChangeCurrency;