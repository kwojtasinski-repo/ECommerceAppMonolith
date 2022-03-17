import styles from './Header.module.css';

function Header(props) {
    return (
        <div className='top-sticky-bar-container js-top-sticky-bar-container'>
            <header className={`${styles.header} header-fixed-container header-fixed-container-visible bg-dark text-white`}>
                <div className='container-fluid'>
                    <div className='d-flex justify-content-center align-items-center ms-2'>
                        <div>
                            ECommerce App
                        </div>

                        <div className='ms-2 text-center'>
                            {props.children}
                        </div>
                    </div>
                </div>
            </header>
        </div>
    );
}

export default Header;