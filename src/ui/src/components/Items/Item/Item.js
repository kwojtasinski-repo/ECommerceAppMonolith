function Item(props) {
    return (
        <div className={`card `}>
            <div className="card-body">
               <div className="row">
                    <div className="col-4">
                        <img
                        src={`https://placeimg.com/220/18${Math.floor(Math.random() * 10)}/arch`}
                        alt=""
                        className="img-fluid img-thumbnail" />
                    </div>
                    <div className="col-8">
                        <div className="row">
                        <div className="col">
                            <p>{props.name}</p>
                        </div>
                        <div className="col text-right">
                            <a className={`btn btn-primary mt-2 px-4`}>
                                Pokaż
                            </a>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Item;