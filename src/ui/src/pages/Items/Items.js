import { NavLink } from "react-router-dom";

function Items(props) {
    return (
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
    )
}

export default Items;