import { NavLink } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { Color } from '../Notification/Notification';
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
                    <NavLink to='/' className={style.menuItem}  >
                        Home
                    </NavLink>
                </li>
                <li className={style.menuItem}>
                    <a className={`${style.menuItem} navbar-brand`} href="#">Profile</a>
                </li>
                {auth ?
                    <>
                        <li className={style.menuItem}>
                            <NavLink to="/" onClick={logout} >Wyloguj</NavLink>
                        </li>
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
            </ul>
        </nav>
    );
}

export default Menu;