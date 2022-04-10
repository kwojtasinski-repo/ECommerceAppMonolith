import { useContext, useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import ReducerContext from '../../context/ReducerContext';
import useAuth from '../../hooks/useAuth';
import useCart from '../../hooks/useCart';
import useNotification from '../../hooks/useNotification';
import { Color } from '../Notification/Notification';
import ItemCart from '../UI/ItemCart/ItemCart';
import style from './Menu.module.css';

function Menu() {
    const [auth, setAuth] = useAuth();
    const [notifications, addNotification] = useNotification();
    const [cart] = useCart();
    const [itemsInCart, setItemsInCart] = useState(0);
    const context = useContext(ReducerContext);
    debugger;
    useEffect(() => {
        setItemsInCart(cart.length);
        
    }, [context.state.cartChanged])

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
                    <Link to='/' className={style.menuItem} >
                        Home
                    </Link>
                </li>
                <li className={style.menuItem}>
                    <Link to='/profile' className={`${style.menuItem}`}>
                        Profil
                    </Link>
                </li>
                {auth ?
                    <>
                        <li className={style.menuItem}>
                            <Link to="/" onClick={logout} >Wyloguj</Link>
                        </li>
                        {auth && auth?.claims?.permissions?.find(c => c === "items") ? 
                            <li className={style.menuItem}>
                                <Link to="/items" className={`${style.menuItem}`}>Przedmioty</Link>
                            </li>
                            : null
                        }
                        {auth && auth?.claims?.permissions?.find(c => c === "currencies") ? 
                            <li className={style.menuItem}>
                                <Link to="/currencies" className={`${style.menuItem}`}>Waluty</Link>
                            </li>
                            : null
                        }
                    </> : (<>
                                <li className={style.menuItem}>
                                    <Link to="/register" className={`${style.menuItem}`} >Zarejestruj</Link>
                                </li>
                                <li className={style.menuItem}>
                                    <Link to="/login" className={`${style.menuItem}`} >
                                        Zaloguj
                                    </Link>
                                </li>
                            </>
                    )
                }
                <li className={style.menuItem}>
                    <Link to="/cart" className={`${style.menuItem}`} >
                        <ItemCart count={itemsInCart} color="#206199" />
                    </Link>
                </li>
            </ul>
        </nav>
    );
}

function Link(props) {
    const [onHover, setOnHover] = useState('');

    const onMouseEnter = () => {
        setOnHover(style.onHover)
    };

    const onMouseLeave = () => setOnHover('');

    return (
        <NavLink onMouseEnter={onMouseEnter} onMouseLeave={onMouseLeave} to={props.to} className={`${props.className} ${onHover}`} onClick={props.onClick} >
            {props.children}
        </NavLink>
    )
}

export default Menu;