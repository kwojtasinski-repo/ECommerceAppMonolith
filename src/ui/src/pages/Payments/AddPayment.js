import axios from "../../axios-setup";
import { useCallback, useEffect, useState } from "react";
import { NavLink, Outlet, useNavigate, useParams } from "react-router";
import { mapToOrder } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import useNotification from "../../hooks/useNotification";
import { Color } from "../../components/Notification/Notification";
import { mapToMessage } from "../../helpers/validation";

function AddPayment() {
    const { id } = useParams();
    const [order, setOrder] = useState();
    const [loading, setLoading] = useState(true);
    const addNotification = useNotification().addNotification;
    const navigate = useNavigate();
    const [actions, setActions] = useState([]);
    const [error, setError] = useState('');

    const addAction = action => {
        let newActions = [...actions];

        if (newActions.length === 5) {
            newActions = [];
            newActions.push(action);
        } else {
            newActions.push(action);
        }

        setActions(newActions);
    }

    const fetchOrder = useCallback(async () => {
        try {
            const response = await axios.get(`/sales-module/orders/${id}`);
            setOrder(mapToOrder(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status);
            setError(errorMessage);
        }

        setLoading(false);
    }, [id])

    const clickHandler = async () => {
        await axios.post('/sales-module/payments', {
            orderId: order.id
        });
        const notification = { color: Color.success, id: new Date().getTime(), text: 'Opłacono zamówienie', timeToClose: 5000 };
        addNotification(notification);
        navigate(`/orders/${order.id}`);
    }

    useEffect(() => {
        fetchOrder();
    }, [actions, fetchOrder]);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <>
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : null}

                {order ? 
                <div className="pt-2">
                    {order.paid ? 
                        <div>
                            <div className="alert alert-primary">Zamówienie {order.orderNumber} opłacono</div>
                        </div>
                        : (
                        <div>
                            <div className="table-responsive">
                                <h4>Zamówienie:</h4>
                                <table className="table table-bordered">
                                    <thead className="table-dark">
                                        <tr>
                                            <th scope="col">Numer zamówienia</th>
                                            <th scope="col">Data zamówienia</th>
                                            <th scope="col">Data zatwierdzenia</th>
                                            <th scope="col">Koszt</th>
                                            <th scope="col">Kod waluty</th>
                                            <th scope="col">Kurs</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr key = {order.id}>
                                            <td>{order.orderNumber}</td>
                                            <td>{order.createOrderDate}</td>
                                            <td>{order.orderApprovedDate}</td>
                                            <td>{order.cost}</td>
                                            <td>{order.code}</td>
                                            <td>{order.rate}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div>
                                <Outlet context={{
                                    orderId: order.id,
                                    code: order.code,
                                    setAction: addAction
                                }}/>
                            </div>
                            <div className="text-center mb-5">
                                <button className="btn btn-success text"
                                        onClick={clickHandler}>
                                            Zapłać
                                </button>
                                <NavLink to='change-currency'
                                    className="btn btn-primary ms-2" >
                                    Zmień walutę
                                </NavLink>
                            </div>
                        </div>
                )}
                </div>
                : null}
                </>
            )}
        </>
    )
}

export default AddPayment;