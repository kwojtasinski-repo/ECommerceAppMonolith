import { useEffect, useState } from "react";
import { NavLink, Outlet, useOutletContext, useParams } from "react-router-dom";
import axios from "../../axios-setup";
import { mapToCurrencies } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";

function Currencies(props) {
    const [loading, setLoading] = useState(true);
    const [currencies, setCurrencies] = useState([]);
    const [refresh, setRefresh] = useState(false);

    const getCurrencies = async () => {
        const response = await axios.get("/currencies-module/currencies");
        setCurrencies(mapToCurrencies(response.data));
        setLoading(false);
    }

    const clickHandler = (id) => {
        console.log(id);

        if (window.confirm("Usunąć walutę?")) {
            console.log('deleted');
        }

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