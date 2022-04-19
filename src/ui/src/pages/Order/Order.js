import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { mapToCustomer, mapToOrder } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import styles from "./Order.module.css"
import useAuth from "../../hooks/useAuth";

function Order(props) {
    const { id } = useParams();
    const [order, setOrder] = useState();
    const [loading, setLoading] = useState(true);
    const [auth] = useAuth();
    const [customer, setCustomer] = useState();

    const fetchOrder = async () => {
        const response = await axios.get(`/sales-module/orders/${id}`);
        setOrder(mapToOrder(response.data));
        setLoading(false);
    }

    const fetchCustomer = async () => {
        const response = await axios.get(`/contacts-module/customers/${order.customerId}`);
        setCustomer(mapToCustomer(response.data));
    }

    useEffect(() => {
        fetchOrder();
    }, [])

    useEffect(() => {
        fetchCustomer();
    }, [order]);

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
                        <NavLink className="btn btn-primary"
                                 to={`/payments/add/${order.id}`}>
                                     Przedź do płatności
                        </NavLink>
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
                    <div>
                        {customer !== null && typeof(customer) !== "undefined" ? (
                        <>
                            <h4>Dane kontaktowe:</h4>
                            <table className="table table-bordered">
                                <thead className="table-dark">
                                    <tr>
                                        <th scope="col">Imię</th>
                                        <th scope="col">Nazwisko</th>
                                        <th scope="col">Nazwa firmy</th>
                                        <th scope="col">NIP</th>
                                        <th scope="col">Telefon</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr key = {customer.id}>
                                        <td>{customer.firstName}</td>
                                        <td>{customer.lastName}</td>
                                        <td>{customer.companyName}</td>
                                        <td>{customer.nip}</td>
                                        <td>{customer.phoneNumber}</td>
                                    </tr>
                                </tbody>
                            </table>
                            <table className="table table-bordered">
                                <thead className="table-dark">
                                    <tr>
                                        <th scope="col">Kraj</th>
                                        <th scope="col">Miasto</th>
                                        <th scope="col">Kod pocztowy</th>
                                        <th scope="col">Ulica</th>
                                        <th scope="col">Nr domu</th>
                                        <th scope="col">Nr lokalu</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr key = {customer.address.id}>
                                        <td>{customer.address.countryName}</td>
                                        <td>{customer.address.cityName}</td>
                                        <td>{customer.address.zipCode}</td>
                                        <td>{customer.address.streetName}</td>
                                        <td>{customer.address.buildingNumber}</td>
                                        <td>{customer.address.localeNumber}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </>
                        ) : null}
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