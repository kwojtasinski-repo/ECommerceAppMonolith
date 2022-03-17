import styles from './Header.module.css';

function Header(props) {
    return (
        <header className={`${styles.header} p-3 bg-dark text-white`}>
            <div className="container">
                <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                    <ul className="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                        <li className="nav-link px-2 text-white">ECommerce App</li>
                    </ul>

                    <div>
                        {props.children}
                    </div>
                </div>
            </div>
        </header>
    );
}

export default Header;