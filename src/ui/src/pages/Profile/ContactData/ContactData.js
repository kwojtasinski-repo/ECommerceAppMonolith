import { useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import axios from "../../../axios-setup";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import { mapToCustomers } from "../../../helpers/mapper";
import AddContact from "./AddContact/AddContact";

function ContactData(props) {
    const [loading, setLoading] = useState(true);
    const [addNewContact, setAddNewContact] = useState(false);
    const [customers, setCustomers] = useState([]);
    
    const fetchContacts = async () => {
        const response = await axios.get("/contacts-module/customers/me");
        setCustomers(mapToCustomers(response.data));
        setLoading(false);
    }

    useEffect(() => {
        fetchContacts();
    }, []);

    const addContact = (value) => {
        setAddNewContact(value);
    }

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div>
                    <div>
                        <button className="btn btn-primary" onClick={ () => addContact(true)} disabled={addNewContact} >
                            Dodaj nowy kontakt
                        </button>
                        {addNewContact ?
                        <button className="btn btn-warning ms-2" onClick={ () => addContact(false)} >
                            Anuluj
                        </button>
                        : null}
                    </div>
                    <div>
                        {addNewContact ? 
                        <div className="mt-2">
                            <AddContact />
                        </div>
                        : null}
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