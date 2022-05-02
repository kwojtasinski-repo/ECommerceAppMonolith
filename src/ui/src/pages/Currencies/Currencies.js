import { useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import axios from "../../axios-setup";
import { mapToCurrencies } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import Popup, { Type } from "../../components/Popup/Popup";
import { mapToMessage } from "../../helpers/validation";

function Currencies(props) {
    const [loading, setLoading] = useState(true);
    const [currencies, setCurrencies] = useState([]);
    const [refresh, setRefresh] = useState(false);
    const [isOpen, setIsOpen] = useState(false);
    const [currentId, setCurrentId] = useState();
    const [error, setError] = useState('');

    const getCurrencies = async () => {
        try {
            const response = await axios.get("/currencies-module/currencies");
            setCurrencies(mapToCurrencies(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status);
            setError(errorMessage);
        }

        setLoading(false);
    }

    const clickHandler = (id) => {
        setCurrentId(id);
        setIsOpen(!isOpen);
    }

    const closePopUp = () => {
        setIsOpen(!isOpen);
    }

    const handleDeleteCurrency = () => {
        setIsOpen(!isOpen);

        try {
            axios.delete(`/currencies-module/currencies/${currentId}`);
        } catch(exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status);
            }
            
            setError(errorMessage);
        }

        setRefresh(true);
    }

    useEffect(() => {
        getCurrencies();

        if (refresh) {
            setRefresh(false);
        }
    }, [refresh]);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div className="pt-2">
                    <div>
                        <NavLink className="btn btn-primary" end to="add" >
                            Dodaj walutę
                        </NavLink>
                    </div>

                    {error ? (
                        <div className="alert alert-danger">{error}</div>
                    ) : null}

                    {isOpen && <Popup handleConfirm = {handleDeleteCurrency}
                                      handleClose = {closePopUp}
                                      type = {Type.alert}
                                      content = {<>
                                          <p>Czy chcesz usunąć walutę?</p>
                                      </>}
                    /> }

                    <div className="pt-4">
                        <Outlet context={{ setRefresh }} />    
                    </div>

                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <th scope="col">Kod waluty</th>
                                <th scope="col">Opis</th>
                                <th scope="col">Edytuj</th>
                                <th scope="col">Usuń</th>
                            </tr>
                        </thead>
                        <tbody>
                            {currencies.map(c => (
                                <tr id ={c.id} key={new Date().getTime() + Math.random() + Math.random()}>
                                    <td>{c.code}</td>
                                    <td>{c.description}</td>
                                    <td><NavLink className="btn btn-warning" end to={`/currencies/edit/${c.id}`} >Edytuj</NavLink></td>
                                    <td><button className="btn btn-danger" onClick={() => clickHandler(c.id)}>Usuń</button></td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    
                </div>
            )}
        </>
    )
}

export default Currencies;