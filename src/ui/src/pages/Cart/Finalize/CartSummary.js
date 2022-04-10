import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import styles from './CartSummary.module.css'
import axios from "../../../axios-setup";
import { mapToOrderItems } from "../../../helpers/mapper";

function CartSummary(props) {
    const [items, setItems] = useState(null);
    debugger;
    const fetchCart = async () => {
        const response = await axios.get('sales-module/cart/me');
        debugger;
        const orderItems = mapToOrderItems(response.data);
        setItems(orderItems);
    }

    useEffect(() => {
        fetchCart();
    }, [])

    return (
        <div className={styles.cart}>
            <div className={styles.title} >
                Podsumowanie
            </div>
            <table className="table">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Nazwa</th>
                        <th scope="col">Koszt</th>
                        <th scope="col">Utworzono</th>
                        <th scope="col">Szczegóły</th>
                    </tr>
                </thead>
                <tbody>
                {items ? (items.map(i => (
                    <tr key = {i.id} >
                        <th scope="row"><img className={styles.image} src={i.images.find(i => true)} alt="" /></th>
                        <td>{i.name}</td>
                        <td>{i.cost}</td>
                        <td>{i.created}</td>
                        <td>
                            <NavLink to = {`/items/${i.id}`}>
                                <button className="btn btn-primary">Przejdź do szczegółów</button>
                            </NavLink>
                        </td>
                    </tr> ))) : <></>}        
                </tbody>
            </table>
            <div>
                <NavLink to = '#' >
                    <button className="btn btn-warning mt-2 float-end"
                            style={{ marginRight: "20%" }}>
                            Realizuj zamówienie
                    </button>
                </NavLink>
            </div>
        </div>
    )
}

export default CartSummary;