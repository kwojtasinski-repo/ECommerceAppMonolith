import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { mapToCustomers, mapToOrder } from "../../../helpers/mapper";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import { mapToMessage } from "../../../helpers/validation";

function EditOrder(props) {
    const { id } =  useParams();
    const [order, setOrder] = useState(null);
    const [loading, setLoading] = useState(true);
    const [customers, setCustomers] = useState([]);
    const [customer, setCustomer] = useState(null);
    const [error, setError] = useState('');

    const fetchOrder = async () => {
        try {
            const response = await axios.get(`/sales-module/orders/${id}`);
            setOrder(mapToOrder(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = 'Zamówienia: ';
            const status = exception.response.status;
            const error = exception.response.data.errors;
            errorMessage += mapToMessage(error, status) + '\n';
            setError(errorMessage);
        }
    }

    const fetchContacts = async () => {
        try {
            const response = await axios.get("/contacts-module/customers/me");
        setCustomers(mapToCustomers(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = 'Dane kontaktowe: ';
            const status = exception.response.status;
            const error = exception.response.data.errors;
            errorMessage += mapToMessage(error, status) + '\n';
            setError(errorMessage);
        }
    }

    const fetchData = async () => {
        await fetchOrder();
        await fetchContacts();
        setLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, []);

    useEffect(() => {
        setCustomer(customers.find(c => c.id === order.customerId));
    }, [order, customers]);

    const rowClicked = id => {
        setCustomer(customers.find(c => c.id === id));
        let newOrder = {...order};
        newOrder.customerId = id;
        setOrder(newOrder);
    }

    const changeConcactData = async () => {
        try {
            await axios.patch("/sales-module/orders/customer/change", {
                orderId: order.id,
                customerId: order.customerId
            });
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
            setLoading(false);
        }
    }

    return (
        <>
        {loading ? <LoadingIcon /> : (
            <div className="pt-2">
                {error ? (
                            <div className="alert alert-danger">
                                {error}
                            </div>
                        ) : null}
                {order !== null ?
                <>
                    <div>
                        <button className="btn btn-warning" 
                                onClick={changeConcactData}>
                                    Zmień dane kontaktowe
                        </button>
                    </div>
                    <div className="table-responsive mt-2">
                        <h4>Zamówienie:</h4>
                        <table className="table table-bordered">
                            <thead className="table-dark">
                                <tr>
                                    <th scope="col">Numer zamówienia</th>
                                    <th scope="col">Data zamówienia</th>
                                    <th scope="col">Data zatwierdzenia</th>
                                    <th scope="col">Koszt</th>
                                    <th scope="col">Waluta</th>
                                    <th scope="col">Kurs</th>
                                    <th scope="col">Opłacono</th>
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
                                    <td>{order.paid ? "Tak"
                                            : "Nie"}
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </>
                : null}
                {customers.length > 0 ? 
                <>
                <div>
                    <h5>
                        {customer !== null && typeof(customer) !== "undefined" ? `Wybrano: ${customer.firstName} ${customer.lastName} ${customer.companyName ? customer.companyName : ""}` : "Wybierz kontakt lub dodaj nowy:"}
                    </h5>
                </div>
                <div>
                    <table className="table table-striped">
                        <thead className="table-dar">
                            <tr>
                                <th scope="col">Imię</th>
                                <th scope="col">Nazwisko</th>
                                <th scope="col">Firma</th>
                                <th scope="col">Nazwa Firmy</th>
                                <th scope="col">NIP</th>
                                <th scope="col">Numer kontaktowy</th>
                            </tr>
                        </thead>
                        <tbody>
                            {customers.map(c => (
                                <tr id ={c.id} key={new Date().getTime() + Math.random() + Math.random()} 
                                    onClick = { () => rowClicked(c.id) } 
                                    className = {customer?.id === c.id ? "bg-success" : null} >
                                    <td>{c.firstName}</td>
                                    <td>{c.lastName}</td>
                                    <td>
                                        {c.company ? (
                                                <input type="checkbox" 
                                                        class="custom-control-input text-primary ms-1" 
                                                        onclick="return false;" 
                                                        checked />
                                        )
                                        : null}
                                    </td>
                                    <td>{c.companyName}</td>
                                    <td>{c.nip}</td>
                                    <td>{c.phoneNumber}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
                </> : null}
            </div>
        )}
        </>
    )
}

export default EditOrder;