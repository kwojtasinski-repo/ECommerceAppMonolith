import { useContext, useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import ReducerContext from '../../context/ReducerContext';
import useAuth from '../../hooks/useAuth';
import useCart from '../../hooks/useCart';
import useNotification from '../../hooks/useNotification';
import { Color } from '../Notification/Notification';
import ItemCart from '../UI/ItemCart/ItemCart';
import style from './Menu.module.css';
import { Nav, NavDropdown } from 'react-bootstrap';
import { createGuid } from '../../helpers/createGuid';

function Menu() {
    const [auth, setAuth] = useAuth();
    const [notifications, addNotification] = useNotification();
    const [cart] = useCart();
    const [itemsInCart, setItemsInCart] = useState(0);
    const context = useContext(ReducerContext);
    const [menuOpened, setMenuOpened] = useState(false);
    
    useEffect(() => {
        setItemsInCart(cart.length);
    }, [context.state.events, cart.length])

    const logout = (event) => {
        event.preventDefault();
        setAuth();
        const notification = { color: Color.info, id: new Date().getTime(), text: 'Wylogowano', timeToClose: 5000 };
        addNotification(notification);
    }
    
    const onClick = (event) => {
        if (event.detail === 1 && menuOpened > 0) {
            setMenuOpened(false);
        }
    }

    return (
        <nav className={`${style.menuContainer} navbar navbar-expand-lg navbar-light bg-light`} onClick={onClick} >
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
                            <DropdownMenu classNameMenu = {style.menuItem}
                                mainTitle = "Przedmioty"
                                navItems = {
                                    <>
                                    <Nav.Link as={Link} to="/items" className={`${style.menuItemDropdown}`}>
                                        Niewystawione
                                    </Nav.Link>
                                    <Nav.Link as={Link} end to="/items/sale/" className={`${style.menuItemDropdown}`}>
                                        Wystawione
                                    </Nav.Link>
                                    <Nav.Link as={Link} end to="/brands" className={`${style.menuItemDropdown}`}>
                                        Firmy
                                    </Nav.Link>
                                    <Nav.Link as={Link} end to="/types" className={`${style.menuItemDropdown}`}>
                                        Typy
                                    </Nav.Link>
                                    </>
                                } />

                            : null
                        }
                        {auth && auth?.claims?.permissions?.find(c => c === "currencies") ? 
                            <li className={style.menuItem}>
                                <Link to="/currencies" className={`${style.menuItem}`}>Waluty</Link>
                            </li>
                            : null
                        }
                        <DropdownMenu classNameMenu = {style.menuItem}
                            mainTitle = "Zamówienia"
                            navItems = {
                                <>
                                <Nav.Link as={Link} to="/cart/summary" className={`${style.menuItemDropdown}`}>
                                    Niezrealizowane zamówienie
                                </Nav.Link>
                                <Nav.Link as={Link} to="/orders" className={`${style.menuItemDropdown}`}>
                                    Moje zamówienia
                                </Nav.Link>
                                </>
                            } />
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

export function DropdownMenu(props) {
    const [id, setId] = useState(createGuid('N'));
    const [menuOpened, setMenuOpened] = useState(false);

    const clickMenuHandler = () => {
        setMenuOpened(!menuOpened);
    }
    
    const onClick = (event) => {
        setMenuOpened(false);
    }

    useEffect(() => {
        document.body.addEventListener('click', function(e) {
            if (e.target.id !== id) {
                onClick(e);
            }
        });
    }, [])
    
    return (
        <Nav className={props.classNameMenu}
                onClick={clickMenuHandler}>
            <NavDropdown
                id={id}
                title={props.mainTitle}
                show={menuOpened}>
                    {props.navItems}
            </NavDropdown>
        </Nav>
    )
}

export default Menu;