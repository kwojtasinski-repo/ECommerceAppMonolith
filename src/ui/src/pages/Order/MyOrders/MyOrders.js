import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { mapToOrders } from "../../../helpers/mapper";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import { NavLink } from "react-router";
import { mapToMessage } from "../../../helpers/validation";
import { requestPath } from "../../../constants";

function MyOrders() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const fetchOrders = async () => {
        try {
            const response = await axios.get(requestPath.salesModule.getMyOrders);
            setOrders(mapToOrders(response.data));
            setLoading(false);
        } catch (exception) {
            console.log(exception);
            let errorMessage = 'Zamówienie: ';
            const status = exception.response.status;
            const error = exception.response.data.errors;
            errorMessage += mapToMessage(error, status) + '\n';
            setError(errorMessage);
        }
    }

    useEffect(() => {
        fetchOrders();
    }, [])

    return (
        <>
        {loading ? <LoadingIcon /> : (
            <div className="pt-2">
                {error ? (
                    <div className="alert alert-danger">
                        {error}
                    </div>
                ) : null}
                <div>
                    <div className="table-responsive">
                        <h4>Moje Zamówienia:</h4>
                        <table className="table table-bordered">
                            <thead className="table-dark">
                                <tr>
                                    <th scope="col">Numer zamówienia</th>
                                    <th scope="col">Data zamówienia</th>
                                    <th scope="col">Data zatwierdzenia</th>
                                    <th scope="col">Koszt</th>
                                    <th scope="col">Kod waluty</th>
                                    <th scope="col">Kurs</th>
                                    <th scope="col">Opłacono</th>
                                    <th scope="col">Akcja</th>
                                </tr>
                            </thead>
                            <tbody>
                                {orders.map(order => (
                                    <tr key = {order.id}>
                                        <td>{order.orderNumber}</td>
                                        <td>{order.createOrderDate}</td>
                                        <td>{order.orderApprovedDate}</td>
                                        <td>{order.cost}</td>
                                        <td>{order.code}</td>
                                        <td>{order.rate}</td>
                                        <td>{order.paid ? "Tak"
                                            : "Nie"}
                                        </td>
                                        <td>
                                            <NavLink to={`${order.id}`}>Przejdź do szczegółów</NavLink>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        )}
        </>
    )
}

export default MyOrders;