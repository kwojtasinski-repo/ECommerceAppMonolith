import styles from './Items.module.css'
import Item from './Item/Item'

function Items(props) {
    return (
        <div>
            <h2 className={styles.title}>Oferty:</h2>
            <Item name="Test" />
        </div>
    );
}

export default Items;