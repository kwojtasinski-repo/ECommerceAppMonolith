import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { mapToTypes } from "../../helpers/mapper";
import { mapToMessage } from "../../helpers/validation";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import { NavLink, Outlet } from "react-router-dom";
import Popup, { Type } from "../../components/Popup/Popup";

function Types() {
    const [loading, setLoading] = useState(true);
    const [types, setTypes] = useState([]);
    const [actions, setActions] = useState([]);
    const [isOpen, setIsOpen] = useState(false);
    const [currentId, setCurrentId] = useState();
    const [error, setError] = useState('');

    const getTypes = async () => {
        try {
            const response = await axios.get("/items-module/types");
            setTypes(mapToTypes(response.data));
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

    const clickHandler = (id) => {
        setCurrentId(id);
        setIsOpen(!isOpen);
    }

    const closePopUp = () => {
        setIsOpen(!isOpen);
    }

    const handleDeleteType = async () => {
        setIsOpen(!isOpen);

        try {
            await axios.delete(`/items-module/types/${currentId}`);
        } catch(exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
        }

        addAction(`delete-type-${currentId}`);
    }

    const addAction = (action) => {
        if (actions.length < 5) {
            const actionsToModify = [...actions];
            actionsToModify.push(action);
            setActions(actionsToModify);
            return;
        }

        const actionToDelete = actions.find(a => true);
        const actionsModified = actions.filter(a => a !== actionToDelete);
        actionsModified.push(action);
        setActions(actionsModified);
    }

    useEffect(() => {
        getTypes();
    }, [actions]);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div className="pt-2">
                    <div>
                        <NavLink className="btn btn-primary" end to="add" >
                            Dodaj typ
                        </NavLink>
                    </div>

                    {error ? (
                        <div className="alert alert-danger">{error}</div>
                    ) : null}

                    {isOpen && <Popup handleConfirm = {handleDeleteType}
                                      handleClose = {closePopUp}
                                      type = {Type.alert}
                                      content = {<>
                                          <p>Czy chcesz usunąć typ?</p>
                                      </>}
                    /> }

                    <div className="pt-4">
                        <Outlet context={{ addAction }} />    
                    </div>

                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <th scope="col">Nazwa</th>
                                <th scope="col">Edytuj</th>
                                <th scope="col">Usuń</th>
                            </tr>
                        </thead>
                        <tbody>
                            {types.map(t => (
                                <tr id ={t.id} key={new Date().getTime() + Math.random() + Math.random() + t.id}>
                                    <td>{t.name}</td>
                                    <td><NavLink className="btn btn-warning" end to={`/types/edit/${t.id}`} >Edytuj</NavLink></td>
                                    <td><button className="btn btn-danger" onClick={() => clickHandler(t.id)}>Usuń</button></td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    
                </div>
            )}
        </>
    )
}

export default Types;