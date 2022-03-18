import styles from './Item.module.css'

function Item(props) {
    return (
        <div className={`card ${styles.itemBorder} ms-2 mt-2`}>
            <div className="card-body">
               <div className="row">
                    <div className="col-4 h-auto w-auto">
                        <img
                        src={props.imageUrl}
                        alt=""
                        className="img-fluid img-thumbnail" />
                    </div>
                    <div className="col-4">
                        <div className="row">
                            <div className="col">
                                <h4>{props.name}</h4>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col mt-5">
                                <h4>{props.cost}</h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Item;