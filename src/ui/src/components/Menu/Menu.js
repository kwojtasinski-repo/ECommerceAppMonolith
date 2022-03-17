import style from './Menu.module.css';

function Menu() {
    return (
        <nav className={`${style.menuContainer} navbar navbar-expand-lg navbar-light bg-light`}>
            <ul className={style.menu}>
                <li className={style.menuItem}>
                    <a className={`${style.menuItem} navbar-brand`} href="#">Home</a>
                </li>
                <li className={style.menuItem}>
                    <a className={`${style.menuItem} navbar-brand`} href="#">Profile</a>
                </li>
            </ul>
        </nav>
    );
}

export default Menu;