import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { mapToBrands } from "../../helpers/mapper";
import { mapToMessage } from "../../helpers/validation";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import { NavLink, Outlet } from "react-router";
import Popup, { Type } from "../../components/Popup/Popup";

function Brands() {
    const [loading, setLoading] = useState(true);
    const [brands, setBrands] = useState([]);
    const [actions, setActions] = useState([]);
    const [isOpen, setIsOpen] = useState(false);
    const [currentId, setCurrentId] = useState();
    const [error, setError] = useState('');

    const getBrands = async () => {
        try {
            const response = await axios.get("/items-module/brands");
            setBrands(mapToBrands(response.data));
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

    const handleDeleteBrand = async () => {
        setIsOpen(!isOpen);

        try {
            await axios.delete(`/items-module/brands/${currentId}`);
        } catch(exception) {
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
        }

        addAction(`delete-brand-${currentId}`);
    }

    const addAction = (action) => {
        if (actions.length < 5) {
            const actionsModify = [...actions];
            actionsModify.push(action);
            setActions(actionsModify);
            return;
        }

        const actionToDelete = actions.find(a => true);
        const actionsModified = actions.filter(a => a !== actionToDelete);
        setActions(actionsModified);
    }

    useEffect(() => {
        getBrands();
    }, [actions]);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div className="pt-2">
                    <div>
                        <NavLink className="btn btn-primary" end to="add" >
                            Dodaj firmę
                        </NavLink>
                    </div>

                    {error ? (
                        <div className="alert alert-danger">{error}</div>
                    ) : null}

                    {isOpen && <Popup handleConfirm = {handleDeleteBrand}
                                      handleClose = {closePopUp}
                                      type = {Type.alert}
                                      content = {<>
                                          <p>Czy chcesz usunąć firmę?</p>
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
                            {brands.map(b => (
                                <tr id ={b.id} key={new Date().getTime() + Math.random() + Math.random() + b.id}>
                                    <td>{b.name}</td>
                                    <td><NavLink className="btn btn-warning" end to={`/brands/edit/${b.id}`} >Edytuj</NavLink></td>
                                    <td><button className="btn btn-danger" onClick={() => clickHandler(b.id)}>Usuń</button></td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    
                </div>
            )}
        </>
    )
}

export default Brands;