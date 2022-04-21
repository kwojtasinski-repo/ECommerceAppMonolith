import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { mapToOrder } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import useNotification from "../../hooks/useNotification";
import { Color } from "../../components/Notification/Notification";

function AddPayment(props) {
    const { id } = useParams();
    const [order, setOrder] = useState();
    const [loading, setLoading] = useState(true);
    const [notifications, addNotification] = useNotification();
    const navigate = useNavigate();

    const fetchOrder = async () => {
        const response = await axios.get(`/sales-module/orders/${id}`);
        setOrder(mapToOrder(response.data));
        setLoading(false);
    }

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
    }, []);

    return (
        <>
            {loading ? <LoadingIcon /> : (
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
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr key = {order.id}>
                                            <td>{order.orderNumber}</td>
                                            <td>{order.createOrderDate}</td>
                                            <td>{order.orderApprovedDate}</td>
                                            <td>{order.cost}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div className="text-center mb-5">
                                <button className="btn btn-success text"
                                        onClick={clickHandler}>
                                            Zapłać
                                </button>
                            </div>
                        </div>
                )}
                </div>
            )}
        </>
    )
}

export default AddPayment;