import { NavLink } from "react-router-dom";
import useCart from "../../hooks/useCart";
import styles from "./Cart.module.css";

function Cart(props) {
    const [items] = useCart();
    const disabledButton = items.length > 0 ? false : true;

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
                    <tr>
                        <th scope="row"><img className={styles.image} src={i.imageUrl} alt="" /></th>
                        <td>{i.name}</td>
                        <td>{i.cost}</td>
                        <td>{i.quantity}</td>
                        <td>
                            <NavLink to = {`/items/${i.id}`}>
                                <button className="btn btn-primary">Przejdź do szczegółów</button>
                            </NavLink>
                        </td>
                    </tr> ))}        
                </tbody>
            </table>
            <div>
                <button className="btn btn-warning mt-2 float-end"
                        style={{ marginRight: "20%" }}
                        disabled={disabledButton} >
                        Podsumowanie
                </button>
            </div>
        </div>
    )
}

export default Cart;