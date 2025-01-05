import { useContext, useState } from "react";
import { NavLink, useNavigate } from "react-router";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import useCart from "../../hooks/useCart";
import styles from "./Cart.module.css";
import axios from '../../axios-setup';
import { mapToMessage } from "../../helpers/validation";
import ReducerContext from "../../context/ReducerContext";
import { requestPath } from "../../constants";

function Cart() {
    const { cart, items = [...cart], removeItem, clear } = useCart();
    const [loading, setLoading] = useState(false);
    const disabledButton = items.length > 0 ? false : true;
    const navigate = useNavigate();
    const context = useContext(ReducerContext);
    const [error, setError] = useState('');

    const summaryHandler = async () => {
        setLoading(true);
        const itemSaleIds = mapItemsToSend(items);

        try {
            await axios.post(requestPath.salesModule.acceptCart, {
                itemSaleIds: itemSaleIds,
                currencyCode: "PLN"
            });
            
            clear();
            setLoading(false);
            context.dispatch({ type: "modifiedState", currentEvent: 'clearCart' });
            navigate('summary');
        } catch(exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for (const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }

            console.log(errorMessage);
            setError(errorMessage);
            setLoading(false);
        }
    }

    const mapItemsToSend = (items) => {
        const itemsToSend = [];
        
        for(const key in items) {
            const quantity = items[key].quantity;

            for(let i = 0; i < quantity; i++) {
                itemsToSend.push(items[key].id);
            }
        }
        return itemsToSend;
    }

    const removeItemHandler = (id) => {
        removeItem(id);
        // TODO: refactor to use store
        context.dispatch({ type: "modifiedState", currentEvent: 'removeItemCart' });
    }

    return (
        <div className={styles.cart}>
            <div className={styles.title} >
                Koszyk
            </div>
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            <table className="table">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Nazwa</th>
                        <th scope="col">Koszt</th>
                        <th scope="col">Ilość</th>
                        <th scope="col">Szczegóły</th>
                    </tr>
                </thead>
                <tbody>
                {items.map(i => (
                    <tr key={i.id} >
                        <th scope="row"><img className={styles.image} src={i.imageUrl} alt="" /></th>
                        <td>{i.name}</td>
                        <td>{i.cost}</td>
                        <td>{i.quantity}</td>
                        <td>
                            <NavLink to = {`/items/${i.id}`}>
                                <button className="btn btn-primary">Przejdź do szczegółów</button>
                            </NavLink>
                            <button className="btn btn-danger ms-2"
                                    onClick={() => removeItemHandler(i.id)} >
                                    Usuń
                            </button>
                        </td>
                    </tr> ))}        
                </tbody>
            </table>
            <div>
                <LoadingButton className="btn btn-warning mt-2 float-end"
                        style={{ marginRight: "20%" }}
                        loading={loading}
                        onClick={summaryHandler}
                        disabled={disabledButton} >
                        Podsumowanie
                </LoadingButton>
            </div>
        </div>
    )
}

export default Cart;