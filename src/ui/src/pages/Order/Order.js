import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { mapToOrder } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import styles from "./Order.module.css"
import useAuth from "../../hooks/useAuth";

function Order(props) {
    const { id } = useParams();
    const [order, setOrder] = useState();
    const [loading, setLoading] = useState(true);
    const [auth] = useAuth();

    const fetchOrder = async () => {
        const response = await axios.get(`/sales-module/orders/${id}`);
        setOrder(mapToOrder(response.data));
        setLoading(false);
    }

    useEffect(() => {
        fetchOrder();
    }, [])

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div className="pt-2">
                    <div className="mb-4">
                        <NavLink className="btn btn-primary me-2"
                                 to={`/orders/edit/${order.id}`} >Edycja</NavLink>
                        {auth.claims.permissions.find(p => p === "item-sale") ?
                            <button className="btn btn-primary me-2">Edycja Pozycji</button>
                            : null }
                        <button className="btn btn-primary">Przedź do płatności</button>
                    </div>
                    <div className="table-responsive">
                        <h4>Zamówienie:</h4>
                        <table className="table table-bordered">
                            <thead className="table-dark">
                                <tr>
                                    <th scope="col">Numer zamówienia</th>
                                    <th scope="col">Data zamówienia</th>
                                    <th scope="col">Data zatwierdzenia</th>
                                    <th scope="col">Koszt</th>
                                    <th scope="col">Opłacono</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr key = {order.id}>
                                    <td>{order.orderNumber}</td>
                                    <td>{order.createOrderDate}</td>
                                    <td>{order.orderApprovedDate}</td>
                                    <td>{order.cost}</td>
                                    <td>{order.paid}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    {order.orderItems?.length > 0 ?
                    <div>
                        <h4>Pozycje:</h4>
                        <div className="table-responsive">
                            <table className="table table-bordered">
                                <thead className="table-dark">
                                    <tr>
                                        <th scope="col">#</th>
                                        <th scope="col">Nazwa</th>
                                        <th scope="col">Koszt</th>
                                        <th scope="col">Utworzono</th>
                                        <th scope="col">Szczegóły</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {order.orderItems.map(oi => (
                                        <tr key = {oi.id}>
                                            <td><img className={styles.image} src={oi.images.find(i => true)} alt="" /></td>
                                            <td>{oi.itemName}</td>
                                            <td>{oi.cost} {oi.code}</td>
                                            <td>{oi.created}</td>
                                            <td>
                                                <NavLink to = {`/archive/items/${oi.id}`}>
                                                    <button className="btn btn-primary">Przejdź do szczegółów</button>
                                                </NavLink>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                    : null}
                </div>
            
            )}
        </>
    )
}

export default Order;