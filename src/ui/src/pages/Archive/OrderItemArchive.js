import axios from "../../axios-setup";
import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router";
import styles from './OrderItemArchive.module.css';
import Gallery from "../../components/Gallery/Gallery";
import { mapToOrderItem } from "../../helpers/mapper";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import useWebsiteTitle from "../../hooks/useWebsiteTitle";
import { mapToMessage } from "../../helpers/validation";

function OrderItemArchive() {
    const { id } = useParams();
    const [item, setItem] = useState(null);
    const [loading, setLoading] = useState(true);
    const setTitle = useWebsiteTitle();
    const [error, setError] = useState('');
    const homeTabName = process.env.REACT_APP_HOME_TAB_NAME;

    const fetchItem = useCallback(async () => {
        try {
            const response = await axios.get(`/sales-module/order-items/${id}`);
            setItem(mapToOrderItem(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status);
            setError(errorMessage);
        }

        setLoading(false);
    }, [id])

    useEffect(() => {
        if (item) {
            setTitle(`Przedmiot - ${item.itemName}`);
        }

        return () => {
            setTitle(homeTabName);
        }
    }, [item, setTitle, homeTabName])

    useEffect(() => {
        fetchItem();
    }, [fetchItem]);

    return (
        <>
            {loading ? <LoadingIcon /> : (
                <>
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : 
                <div className={`card ${styles.item}`}>
                    <div className="card-body">
                        <div className="row">
                            <div className="col-4">
                                <img src={item.images.find(i => true)} alt=""
                                    className="img-fluid img-thumbnail"/>
                            </div>
                            <div className="col-8">
                                <div className="row">
                                    <div className="col">
                                        <p className={styles.title}>{item.itemName}</p>
                                        <p className={styles.typeDesc}>Typ:</p>
                                        <p className={styles.type}>{item.typeName}</p>
                                        <p className={styles.brandDesc}>Firma:</p>
                                        <p className={styles.brand}>{item.brandName}</p>
                                        <p className={styles.brandDesc}>Utworzono:</p>
                                        <p className={styles.brand}>{item.created}</p>
                                    </div>
                                    <div>
                                        <div className={`${styles.costTheme} float-end`}>                                    
                                            <p>Cena wystawienia:</p>
                                            <p className={styles.cost}>{item.cost} {item.code}</p>    
                                            <p className={styles.brandDesc}>Kurs:</p>
                                            <p className={styles.brand}>{item.rate}</p>
                                            <p>Cena zakupu:</p>
                                            <p className={styles.cost}>{item.cost} {item.code}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-12">
                                <h5>Opis:</h5>
                                <p className={styles.description}>{item.description}</p>
                            </div>
                            <div className={styles.galery}>
                                <p>Galeria:</p>
                                <Gallery items={item.images} />
                            </div>
                        </div>
                    </div>
                </div>
                }
                </>
                )
            }
        </>
    )
}

export default OrderItemArchive;