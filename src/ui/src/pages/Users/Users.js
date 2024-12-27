import { useEffect, useState } from "react";
import { NavLink } from "react-router";
import axios from "../../axios-setup";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import { mapToUsers } from "../../helpers/mapper";
import { mapToMessage } from "../../helpers/validation";

function Users() {
    const [loading, setLoading] = useState(true);
    const [users, setUsers] = useState([]);
    const [error, setError] = useState('');

    const fetchUsers = async () => {
        try {
            const response = await axios.get('/users-module/accounts');
            setUsers(mapToUsers(response.data));
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
        fetchUsers();
    }, []);

    return (
        <div>
            {loading ? <LoadingIcon /> :
                <>
                    <div className="mt-2 mb-2">
                        <h5>Użytkownicy</h5>
                    </div>
                    {error ? (
                        <div className="alert alert-danger mb-2">
                            {error}
                        </div>
                    ) : null}
                    <div className="table-responsive">
                        <table className="table table-bordered">
                            <thead className="table-dark">
                                <tr>
                                    <th scope="col">Email</th>
                                    <th scope="col">Rola</th>
                                    <th scope="col">Utworzony</th>
                                    <th scope="col">Aktywny</th>
                                    <th scope="col">Akcja</th>
                                </tr>
                            </thead>
                            <tbody>
                                {users.map(u => (   
                                    <tr key = {u.id} >
                                        <td>{u.email}</td>
                                        <td>{u.role}</td>
                                        <td>{u.createdAt}</td>
                                        <td>{u.isActive ? "Tak" : "Nie"}</td>
                                        <td>
                                            <NavLink to={`edit/${u.id}`} className="btn btn-warning me-2">Edytuj</NavLink>
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

export default Users;