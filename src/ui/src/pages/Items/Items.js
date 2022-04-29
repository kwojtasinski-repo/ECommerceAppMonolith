import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import { mapToItemsDetails } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import style from "./Items.module.css";

function Items(props) {
    const [loading, setLoading] = useState(true);
    const [items, setItems] = useState([]);

    const fetchItems = async () => {
        const response = await axios.get('/items-module/items');
        setItems(mapToItemsDetails(response.data));
        setLoading(false);
    }

    useEffect(() => {
        fetchItems();
    }, [])

    return (
        <div>
            <div className="row">
                <div className="d-flex justify-content-center mb-2">
                    <NavLink className="btn btn-success" to='add'>Dodaj przedmiot</NavLink>
                </div>
                <div className="d-flex justify-content-center mb-2">
                    <NavLink className="btn btn-warning" to='edit'>Edytuj przedmiot</NavLink>
                </div>
                <div className="d-flex justify-content-center mb-2">
                    <NavLink className="btn btn-primary" to='for-sale'>Wystaw przedmiot</NavLink>
                </div>
                <div className="d-flex justify-content-center mb-2">
                    <NavLink className="btn btn-danger" to='delete'>Usuń przedmiot</NavLink>
                </div>
            </div>

            {loading ? <LoadingIcon /> :
                <div className="table-responsive">
                    <table className="table table-bordered">
                        <thead className="table-dark">
                            <tr>
                                <th scope="col">#</th>
                                <th scope="col">Nazwa</th>
                                <th scope="col">Firma</th>
                                <th scope="col">Typ</th>
                                <th scope="col">Akcja</th>
                            </tr>
                        </thead>
                        <tbody>
                            {items.map(i => (   
                                <tr key = {i.id}>
                                    <td className={style.imageRow}><img className={style.image} src = {i.imageUrl?.url} alt="" /></td>
                                    <td>{i.name}</td>
                                    <td>{i.brand}</td>
                                    <td>{i.type}</td>
                                    <td>
                                        <NavLink className="btn btn-warning me-2" to = {`edit/${i.id}`}>Edytuj</NavLink>
                                        <button className="btn btn-danger">Usuń</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            }
            </div>
    )
}

export default Items;