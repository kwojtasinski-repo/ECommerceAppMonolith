import { useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import axios from "../../../axios-setup";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import { mapToCustomers } from "../../../helpers/mapper";

function ContactData(props) {
    const [loading, setLoading] = useState(true);
    const [customers, setCustomers] = useState([]);
    
    const fetchContacts = async () => {
        const response = await axios.get("/contacts-module/customers/me");
        setCustomers(mapToCustomers(response.data));
        setLoading(false);
    }

    useEffect(() => {
        fetchContacts();
    }, []);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div>
                    <div>
                        <NavLink to = "add" className="btn btn-primary mb-2" >Dodaj nowy kontakt</NavLink>
                    </div>
                    <div className="mt-2">
                        <Outlet />
                    </div>
                    <div>
                        <table className="table table-striped">
                            <thead>
                                <tr>
                                    <th scope="col">Imię</th>
                                    <th scope="col">Nazwisko</th>
                                    <th scope="col">Firma</th>
                                    <th scope="col">Nazwa Firmy</th>
                                    <th scope="col">NIP</th>
                                    <th scope="col">Numer kontaktowy</th>
                                    <th scope="col">Akcja</th>
                                </tr>
                            </thead>
                            <tbody>
                                {customers.map(c => (
                                    <tr id ={c.id} key={new Date().getTime() + Math.random() + Math.random()}>
                                        <td>{c.firstName}</td>
                                        <td>{c.lastName}</td>
                                        <td>{c.company}</td>
                                        <td>{c.companyName}</td>
                                        <td>{c.nip}</td>
                                        <td>{c.phoneNumber}</td>
                                        <td><NavLink className="btn btn-warning" end to={`edit/${c.id}`} >Edytuj</NavLink></td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}
        </>
    )
}

export default ContactData;