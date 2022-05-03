import axios from "../../../axios-setup";
import { useEffect, useRef, useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { mapToItems } from "../../../helpers/mapper";
import { mapToMessage } from "../../../helpers/validation";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import style from "./ItemsForSale.module.css";
import Popup, { Type } from "../../../components/Popup/Popup";

function ItemsForSale(props) {
    const { term } = useParams();
    const [termSearch, setTermSearch] = useState(term);
    const [searchString, setSearchString] = useState(term);
    const [items, setItems] = useState([]);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);
    const inputRef = useRef(null);
    const [currentId, setCurrentId] = useState('');
    const [isOpen, setIsOpen] = useState(false);
    const [actions, setActions] = useState([]);

    const onKeyDownHandler = event => {
        if (event.key === 'Enter') {
            search();
        }
    }

    const search = () => {
        setTermSearch(searchString);
    }

    const searchHandler = async () => {
        try {
            const url = term ? `/items-module/item-sales/search?name=${term}` : '/items-module/item-sales';
            const response = await axios.get(url);
            setItems(mapToItems(response.data));
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
        searchHandler();
    }, [termSearch, actions])

    const clickHandler = (id) => {
        setCurrentId(id);
        setIsOpen(!isOpen);
    }
    
    const handleDeleteItem = async () => {
        setIsOpen(!isOpen);

        try {
            await axios.delete(`/items-module/item-sales/${currentId}`);
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
            <div className="d-flex mb-2">
                <input 
                    ref={inputRef}
                    value={searchString}
                    onKeyDown={onKeyDownHandler}
                    onChange={event => setSearchString(event.target.value)}
                    type = "text"
                    className = "form-control search flex-fill"
                    placeholder = "Szukaj..."/>
                <button className = {`ms-1 btn btn-primary`}
                    onClick={search}>Szukaj</button>
            </div>
            {searchString ?
                <div className="mt-2 mb-2">
                    Wyniki wyszukiwania: {searchString}
                </div>
                : 
            null}
            {loading ? <LoadingIcon /> :
                <>
                    {error ? (
                        <div className="alert alert-danger mb-2">
                            {error}
                        </div>
                    ) : null}
                    {isOpen && <Popup handleConfirm = {handleDeleteItem}
                                        handleClose = {closePopUp}
                                        type = {Type.alert}
                                        content = {<>
                                            <p>Czy chcesz usunąć przedmiot?</p>
                                        </>}
                    /> }
                    <div className="table-responsive">
                        <table className="table table-bordered">
                            <thead className="table-dark">
                                <tr>
                                    <th scope="col">#</th>
                                    <th scope="col">Nazwa</th>
                                    <th scope="col">Cena</th>
                                    <th scope="col">Akcja</th>
                                </tr>
                            </thead>
                            <tbody>
                                {items.map(i => (   
                                    <tr key = {i.id} >
                                        <td className={style.imageRow}><img className={style.image} src = {i.imageUrl} alt="" /></td>
                                        <td>{i.name}</td>
                                        <td>{i.cost} {i.code}</td>
                                        <td>
                                            <NavLink to={`/items/sale/edit/${i.id}`} className="btn btn-warning me-2">Edytuj</NavLink>
                                        <button className="btn btn-danger" onClick={() => clickHandler(i.id)}>Usuń</button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </>
            }
        </div>
    )
}

export default ItemsForSale;