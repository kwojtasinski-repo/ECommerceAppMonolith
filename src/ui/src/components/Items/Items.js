import styles from './Items.module.css'
import Item from './Item/Item'
import { createGuid } from '../../helpers/createGuid';

function Items(props) {
    const id = createGuid();
    const cost = 1500;
    const item = {
        id: id,
        name: "Iphone 13s Pro",
        cost: cost,
        imageUrl: `https://placeimg.com/220/18${Math.floor(Math.random() * 10)}/arch`
    }

    return (
        <div>
            <h1 className={styles.title}>Oferty:</h1>
            <div className='row me-2'>
                {props.items.map(i => 
                            <Item key={i.id}
                                  {...i} />
                )}
            </div>
            
        </div>
    );
}

export default Items;