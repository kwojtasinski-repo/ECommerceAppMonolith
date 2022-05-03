import axios from "../../../axios-setup";
import { NavLink, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { mapToItemDetails } from "../../../helpers/mapper";
import LoadingIcon from "../../../components/UI/LoadingIcon/LoadingIcon";
import Gallery from "../../../components/Gallery/Gallery";
import styles from "./ItemDetails.module.css";
import { mapToMessage } from "../../../helpers/validation";
import Tags from "../../../components/Tags/Tags";

function ItemDetails(props) {
    const { id } = useParams();
    const [item, setItem] = useState(null);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);
    const [mainImage, setMainImage] = useState(null);

    const fetchItem = async () => {
        try {
            const response = await axios.get(`/items-module/items/${id}`);
            const itemLocal = mapToItemDetails(response.data);
            setItem(itemLocal);
            const imageMain = itemLocal.imagesUrl?.find(im => im.mainImage)?.url;
            
            if (imageMain) {
                setMainImage(imageMain);
            }
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response?.status;
            const errors = exception.response?.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';            
            setError(errorMessage);
        }
        
        setLoading(false);
    }

    useEffect(() => {
        fetchItem();
    }, []);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <div className={`card ${styles.item}`}>
                    <div className="card-body">
                        {error ? (
                            <div className="alert alert-danger">{error}</div>
                        ) : null}
                        <div className="row">
                            <div className="col-4">
                                {mainImage ?
                                    <img src={mainImage} alt=""
                                         className="img-fluid img-thumbnail"/>
                                    : <img src="data:," alt=""></img>
                                }
                            </div>
                            <div className="col-8">
                                <div className="row">
                                    <div className="col">
                                        <p className={styles.title}>{item.name}</p>
                                        <p className={styles.typeDesc}>Typ:</p>
                                        <p className={styles.type}>{item.type}</p>
                                        <p className={styles.brandDesc}>Firma:</p>
                                        <p className={styles.brand}>{item.brand}</p>
                                    </div>
                                </div>
                            </div>
                            <div className="col-12">
                                <h5>Opis:</h5>
                                <p className={styles.description}>{item.description}</p>
                            </div>
                            <div>
                                <Tags tags = {item.tags}
                                      canEdit = {false} />
                            </div>
                            <div className={styles.galery}>
                                <p>Galeria:</p>
                                <Gallery items={item.imagesUrl.filter(im => !im.mainImage).map(im => im.url)} />
                            </div>
                        </div>
                        <div>
                            <NavLink type="button" 
                                    className="btn btn-warning float-end"
                                    to = {`/items/edit/${id}`} >
                                    Edytuj
                            </NavLink>
                        </div>
                    </div>
                </div>
            )}
        </>
    )
}

export default ItemDetails;