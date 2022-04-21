import { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import styles from './CartSummary.module.css'
import axios from "../../../axios-setup";
import { mapToOrderItems } from "../../../helpers/mapper";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";

function CartSummary(props) {
    const [items, setItems] = useState(null);
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);

    const fetchCart = async () => {
        const response = await axios.get('sales-module/cart/me');
        const orderItems = mapToOrderItems(response.data);
        setItems(orderItems);
        setLoading(false);
    }

    const removeItemHandler = async (id) => {
        await axios.delete(`sales-module/order-items/${id}`);
        navigate(0); // refresh page
    }

    const calculateCost = (items) => {
        let sum = 0;

        for (const key in items) {
            sum += items[key].cost;
        }

        return sum;
    }

    useEffect(() => {
        fetchCart();
    }, [])

    return loading ? <LoadingIcon /> : (
        <>
        {items && items.length > 0 ? 
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
                            <td>{i.itemName}</td>
                            <td>{i.cost} {i.code}</td>
                            <td>{i.created}</td>
                            <td>
                                <NavLink to = {`/archive/items/${i.id}`}>
                                    <button className="btn btn-primary">Przejdź do szczegółów</button>
                                </NavLink>
                                <button className="btn btn-danger ms-2"
                                        onClick={() => removeItemHandler(i.id)} >
                                        Usuń
                                </button>
                            </td>
                        </tr> ))) : <></>}        
                    </tbody>
                </table>
                {items ? (
                    <div>
                        <table className="table w-25" style={{marginLeft: 'auto', marginRight: '0'}}>
                            <thead>
                                <tr>
                                    <th>Razem: </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr key = {new Date().getTime()}>
                                    <td>{calculateCost(items)} PLN</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    ) : <></>
                }
                <div>
                    <NavLink to = '/orders/add' >
                        <button className="btn btn-success mt-2 float-end" >
                                Realizuj zamówienie
                        </button>
                    </NavLink>
                </div>
            </div>
        :  <h4>Aktualnie nie masz żadnych przedmiotów w trakcie realizacji zamówienia</h4>}
        </> 
    )
}

export default CartSummary;