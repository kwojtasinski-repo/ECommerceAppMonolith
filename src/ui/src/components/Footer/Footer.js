import styles from './Footer.module.css'

function Footer() {
    const year = new Date().getFullYear();
    
    return(
        <div className={styles.footer}>
            <div className="text-center m-3 text-primary">
                &copy; Kamil Wojtasiński {year}
            </div>
        </div>
    );
}

export default Footer;