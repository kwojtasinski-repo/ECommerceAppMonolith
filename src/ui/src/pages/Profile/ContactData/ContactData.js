import { useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router";
import axios from "../../../axios-setup";
import Popup, { Type }  from "../../../components/Popup/Popup";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import { mapToCustomers } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import { requestPath } from "../../../constants";

function ContactData() {
    const showDataId = 'show-data';
    const [loading, setLoading] = useState(true);
    const [customers, setCustomers] = useState([]);
    const [currentId, setCurrentId] = useState();
    const [isOpen, setIsOpen] = useState(false);
    const [error, setError] = useState(false);
    const [actions, setActions] = useState([]);
    
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

    const fetchContacts = async () => {
        try {
            const response = await axios.get(requestPath.contactsModule.getMyCustomers);
            setCustomers(mapToCustomers(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }

        setLoading(false);
    }

    const scrollToContactData = () => {
        document.getElementById(showDataId).scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    const clickHandler = (id) => {
        setCurrentId(id);
        setIsOpen(!isOpen);
    }

    const handleDeleteContact = async () => {
        setIsOpen(!isOpen);
        try {
            await axios.delete(requestPath.contactsModule.deleteCustomer(currentId));
        } catch(exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
        }
        addAction('deleteContact');
    }

    const closePopUp = () => {
        setIsOpen(!isOpen);
    }

    useEffect(() => {
        fetchContacts();
    }, [actions]);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div>
                    <div>
                        <NavLink to = "add" className="btn btn-primary mb-2" >Dodaj nowy kontakt</NavLink>
                    </div>

                    <div id={showDataId}></div>

                    {error ? (
                        <div className="alert alert-danger">{error}</div>
                    ) : null}

                    {isOpen && <Popup handleConfirm = {handleDeleteContact}
                                      handleClose = {closePopUp}
                                      type = {Type.alert}
                                      content = {<>
                                          <p>Czy chcesz usunąć kontakt?</p>
                                      </>}
                    /> }

                    <div className="mt-2">
                        <Outlet context={{ addAction }}/>
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
                                    <th scope="col">Edytuj</th>
                                    <th scope="col">Usuń</th>
                                </tr>
                            </thead>
                            <tbody>
                                {customers.map(c => (
                                    <tr id ={c.id} key={new Date().getTime() + Math.random() + Math.random()}>
                                        <td>{c.firstName}</td>
                                        <td>{c.lastName}</td>
                                        <td>
                                            {c.company ? (
                                                  <input type="checkbox" 
                                                         className="custom-control-input text-primary ms-1" 
                                                         onClick={() => false}
                                                         onChange={() => {}}
                                                         checked={true} />
                                            )
                                            : null}
                                        </td>
                                        <td>{c.companyName}</td>
                                        <td>{c.nip}</td>
                                        <td>{c.phoneNumber}</td>
                                        <td>
                                            <NavLink className="btn btn-warning" end to={`edit/${c.id}`} onClick={() => scrollToContactData()}>Edytuj</NavLink>
                                        </td>
                                        <td>
                                            <button className="btn btn-danger" onClick={() => clickHandler(c.id)}>Usuń</button>
                                        </td>
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