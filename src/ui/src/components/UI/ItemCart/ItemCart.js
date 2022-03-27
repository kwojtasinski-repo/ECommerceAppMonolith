import { library } from '@fortawesome/fontawesome-svg-core';
import { fas, faCartShopping } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from 'react';
import styles from './ItemCart.module.css';

library.add(fas, faCartShopping);

function ItemCart(props) {
    const [background, setBackground] = useState('#94989a');

    return (
        <div onMouseEnter={() => setBackground(props.color)}
             onMouseLeave={() => setBackground('#94989a')}
              >
            {props.count ?
                <span className={`badge badge-light ${styles.itemCartCount}`} style={{ color : props.color}}>{props.count}</span>
                : null }
            <FontAwesomeIcon icon={faCartShopping} className={styles.shop} style={{ color : props.color}}/>
        </div>
    )
}

export default ItemCart;