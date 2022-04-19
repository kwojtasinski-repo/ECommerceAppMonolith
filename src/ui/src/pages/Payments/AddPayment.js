import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { mapToOrder } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";

function AddPayment(props) {
    const { id } = useParams();
    const [order, setOrder] = useState();
    const [loading, setLoading] = useState(true);

    const fetchOrder = async () => {
        const response = await axios.get(`/sales-module/orders/${id}`);
        setOrder(mapToOrder(response.data));
        setLoading(false);
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
                                <button className="btn btn-success text">Opłać</button>
                            </div>
                        </div>
                )}
                </div>
            )}
        </>
    )
}

export default AddPayment;