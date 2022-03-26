import { NavLink } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { Color } from '../Notification/Notification';
import ItemCart from '../UI/ItemCart/ItemCart';
import style from './Menu.module.css';

function Menu() {
    const [auth, setAuth] = useAuth();
    const [notifications, addNotification] = useNotification();

    const logout = (event) => {
        event.preventDefault();
        setAuth();
        const notification = { color: Color.info, id: new Date().getTime(), text: 'Wylogowano', timeToClose: 5000 };
        addNotification(notification);
    }

    return (
        <nav className={`${style.menuContainer} navbar navbar-expand-lg navbar-light bg-light`}>
            <ul className={style.menu}>
                <li className={style.menuItem}>
                    <NavLink to='/' className={style.menuItem} >
                        Home
                    </NavLink>
                </li>
                <li className={style.menuItem}>
                    <NavLink to='/profile'>
                        Profil
                    </NavLink>
                </li>
                {auth ?
                    <>
                        <li className={style.menuItem}>
                            <NavLink to="/" onClick={logout} >Wyloguj</NavLink>
                        </li>
                        {auth && auth?.claims?.permissions?.find(c => c === "items") ? 
                            <li className={style.menuItem}>
                                <NavLink to="/items">Przedmioty</NavLink>
                            </li>
                            : null
                        }
                    </> : (<>
                                <li className={style.menuItem}>
                                    <NavLink to="/register" >Zarejestruj</NavLink>
                                </li>
                                <li className={style.menuItem}>
                                    <NavLink to="/login" >
                                        Zaloguj
                                    </NavLink>
                                </li>
                            </>
                    )
                }
                <NavLink to="#" className={style.menuItem} >
                    <ItemCart />
                </NavLink>
            </ul>
        </nav>
    );
}

export default Menu;