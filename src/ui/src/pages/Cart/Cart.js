import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import useCart from "../../hooks/useCart";
import styles from "./Cart.module.css";
import axios from '../../axios-setup';
import { mapToMessage } from "../../helpers/validation";

function Cart(props) {
    const [items, addItem, removeItem] = useCart();
    const [loading, setLoading] = useState(false);
    const disabledButton = items.length > 0 ? false : true;
    const navigate = useNavigate();

    const summaryHandler = async () => {
        setLoading(true);
        const itemSaleIds = mapItemsToSend(items);

        try {
            await axios.post('sales-module/order-items/multi', {
                itemSaleIds: itemSaleIds,
                currencyCode: "PLN"
            });
    
            setLoading(false);
            navigate('summary');
        } catch(exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status);
            }

            console.log(errorMessage);
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
        debugger;
        return itemsToSend;
    }

    const removeItemHandler = (id) => {
        removeItem(id);
        navigate(0); // refresh page
    }

    return (
        <div className={styles.cart}>
            <div className={styles.title} >
                Koszyk
            </div>
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