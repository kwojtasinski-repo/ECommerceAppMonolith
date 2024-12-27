import styles from './Item.module.css'
import { Link } from 'react-router';
import useCart from '../../../hooks/useCart';
import ReducerContext from '../../../context/ReducerContext';
import { useContext } from 'react';

function Item(props) {
    const addItem = useCart().addItem;
    const context = useContext(ReducerContext);

    const onClickHandler = (item) => {
        addItem(item);
        context.dispatch({ type: "modifiedState", currentEvent: 'addItem' });
    }

    return (
        <div className={`card ${styles.itemBorder} ms-2 mt-2`}>
            <div className="card-body">
               <div className="row">
                    <div className="col-4 h-auto w-auto">
                        <Link to={`/items/${props.id}`} >
                            <img
                                src={props.imageUrl}
                                alt=""
                                className={`img-fluid img-thumbnail ${styles.imgMax}`} />
                        </Link>
                    </div>
                    <div className="col-4">
                        <div className="row">
                            <div className="col">
                                <h4>{props.name}</h4>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col mt-5">
                                <p>Cena:</p>
                                <h4>{props.cost} PLN</h4>
                            </div>
                        </div>
                    </div>
                    <div>
                        <button className={`btn btn-primary mt-2 px-5 float-end`} style={{width: '14rem'}} onClick={onClickHandler.bind(this, props)} >
                            Dodaj do koszyka
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Item;