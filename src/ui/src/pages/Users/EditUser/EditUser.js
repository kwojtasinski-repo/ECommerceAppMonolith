import axios from "../../../axios-setup";
import { NavLink, useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { mapToUser } from "../../../helpers/mapper";
import { mapToMessage, validate } from "../../../helpers/validation";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import Input from "../../../components/Input/Input";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";

function EditUser(props) {
    const { id } = useParams();
    const [user, setUser] = useState(null);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);
    const [loadinButton, setLoadingButton] = useState(false);
    const navigate = useNavigate();
    const claims = [
    {
        label: "Waluty",
        value: "currencies"
    } , 
    {   
        label: "Przedmioty",
        value: "items"
    }, 
    {   
        label: "Przedmioty na sprzedaż",
        value: "item-sale"
    }, 
    {   
        label: "Użytkownicy",
        value: "users"
    }];
    const [form, setForm] = useState({
        id: {
            value: ''
        },
        role: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        claims: {
            value: [],
            error: '',
            showError: false,
            rules: []
        }
    });

    const fetchUser = async () => {
        try {
            const response = await axios.get(`/users-module/accounts/${id}`);
            const userLocal = mapToUser(response.data);
            setUser(userLocal);
            setForm({
                ...form,
                id: {
                    value: userLocal.id
                },
                role: {
                    ...form.role,
                    value: userLocal.role
                },
                claims: {
                    ...form.claims,
                    value: userLocal.claims.permissions
                }
            })
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

    const changeHandler = (value, fieldName) => {
        const error = validate(form[fieldName].rules, value);
        setForm({
            ...form, 
            [fieldName]: {
                ...form[fieldName],
                value,
                showError: true,
                error: error
            } 
        });
    };

    useEffect(() => {
        fetchUser();
    }, []);

    const onSubmit = async (event) => {
        event.preventDefault();
        setLoadingButton(true);

        try {
            await axios.put('/users-module/accounts/policies', {
                userId: id,
                role: form.role.value,
                claims: form.claims.value
            });
            navigate('/users');
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
            setLoadingButton(false);
        }
    }

    const changeActive = async (active) => {
        try {
            await axios.patch('/users-module/accounts/active', {
                userId: id,
                active
            });
            navigate('/users');
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            
            setError(errorMessage);
        }
    }

    return (
        <div>
            {loading ? <LoadingIcon /> :
                <div>
                    <h5>Edycja użytkownika</h5>
                    {user && user?.isActive ? 
                        <button className="btn btn-danger mt-2 mb-2"
                                onClick={() => changeActive(false)}>
                                    Dezaktywuj
                        </button>
                        : <button className="btn btn-success mt-2 mb-2"
                                  onClick={() => changeActive(true)} >
                                      Aktywuj
                        </button>
                    }
                    {error ? (
                        <div className="alert alert-danger mb-2">
                            {error}
                        </div>
                    ) : null}
                    {user ? 
                        <div>
                            <div>
                                <div className="form-group">
                                    <label>Email</label>
                                    <input type = "text"
                                        value = {user.email}
                                        className = "form-control"
                                        readOnly />
                                </div>
                                <div className="form-group">
                                    <label>Utworzony</label>
                                    <input type = "text"
                                        value = {user.createdAt}
                                        className = "form-control"
                                        readOnly />
                                </div>
                                <div className="row">
                                    <div className="form-group">
                                        <label>Aktywny: {user.isActive ? "Tak" : "Nie"}</label>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <form className="mt-2" onSubmit={onSubmit}>
                                    <Input label = "Rola"
                                           type = "text"
                                           value = {form.role.value}
                                           error = {form.role.error}
                                           showError = {form.role.showError}
                                           onChange = {val => changeHandler(val, 'role')} />

                                    <div className="mt-2 mb-2">
                                        <h5>Uprawnienia</h5>
                                        <Input type = "checkbox"
                                               value = {form.claims.value}
                                               options = {claims}
                                               onChange = {val => changeHandler(val, 'claims')} />
                                    </div>                                            
                                </form>
                            </div>
                            <div className="text-end mt-2">
                                <LoadingButton type="submit" className="btn btn-success me-2"
                                               onClick={onSubmit}
                                               loading={loadinButton} >
                                            Zatwierdź
                                </LoadingButton>
                                <NavLink type="button" className="btn btn-secondary" to='/users'>Anuluj</NavLink>
                            </div>
                        </div>
                    : null}
                </div>
            }
        </div>
    )
}

export default EditUser;