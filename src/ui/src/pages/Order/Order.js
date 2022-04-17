import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { mapToOrder } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";

function Order(props) {
    const { id } = useParams();
    const [order, setOrder] = useState();
    const [loading, setLoading] = useState(true);

    const fetchOrder = async () => {
        debugger;
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
                    <div>
                        {order.orderNumber}
                    </div>
                </div>
            
            )}
        </>
    )
}

export default Order;