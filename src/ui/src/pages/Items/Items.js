import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import { mapToItemsDetails } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import style from "./Items.module.css";
import { mapToMessage } from "../../helpers/validation";
import Popup, { Type } from "../../components/Popup/Popup";

function Items() {
    const [loading, setLoading] = useState(true);
    const [items, setItems] = useState([]);
    const [isOpen, setIsOpen] = useState(false);
    const [currentId, setCurrentId] = useState('');
    const [error, setError] = useState('');
    const [actions, setActions] = useState([]);

    const fetchItems = async () => {
        try {
            const response = await axios.get('/items-module/items/not-put-up-for-sale');
            setItems(mapToItemsDetails(response.data));
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

    useEffect(() => {
        fetchItems();
    }, [actions]);

    const clickHandler = (id) => {
        setCurrentId(id);
        setIsOpen(!isOpen);
    }

    const handleDeleteItem = async () => {
        setIsOpen(!isOpen);

        try {
            await axios.delete(`/items-module/items/${currentId}`);
        } catch(exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
        }

        addAction(`deleteItem-${currentId}`);
    }

    const addAction = (actionToAdd) => {
        if (actions.length < 5) {
            const actionsModifed = [...actions];
            actionsModifed.push(actionToAdd);
            setActions(actionsModifed);
            return;
        }

        const actionToDelete = actions.find(i => true);
        let actionsModified = actions.filter(a => a !== actionToDelete);
        actionsModified.push(actionToAdd);
        setActions(actionsModified);
    }

    const closePopUp = () => {
        setIsOpen(!isOpen);
    }

    return (
        <div>
            <div>
                <NavLink className="btn btn-success mb-4" to='add'>Dodaj przedmiot</NavLink>
            </div>
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            {isOpen && <Popup handleConfirm = {handleDeleteItem}
                                handleClose = {closePopUp}
                                type = {Type.alert}
                                content = {<>
                                    <p>Czy chcesz usunąć przedmiot?</p>
                                </>}
            /> }
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
                                        <NavLink className="btn btn-success me-2" to = {`for-sale/${i.id}`}>Wystaw przedmiot</NavLink>
                                        <NavLink className="btn btn-primary me-2" to = {`details/${i.id}`}>Szczegóły</NavLink>
                                        <NavLink className="btn btn-warning me-2" to = {`edit/${i.id}`}>Edytuj</NavLink>
                                        <button className="btn btn-danger" onClick={() => clickHandler(i.id)}>Usuń</button>
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