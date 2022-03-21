import { useEffect, useState } from "react";
import styles from './Gallery.module.css';

function Gallery(props) {
    const [isOpen, setIsOpen] = useState(false);
    const [items, setItems] = useState([]);
    const [imageToShow, setImageToShow] = useState('');

    useEffect(() => {
        if (props.items) {
            const itemArrays = props.items.map(i => (
                { id: new Date().getTime() + Math.floor(Math.random() * 10), url: i }
            ));
            setItems(itemArrays);
        }
    }, []);

    const handleShowDialog = (event) => {
        if (!isOpen) {
            setImageToShow(items.find(i => i.id === Number(event.target.id)).url);
        }
        
        setIsOpen(!isOpen);
    };

    return (
        <div className="overlay">
            {items.map(i => (
                    <img key={new Date().getTime() + Math.random()} id={i.id} onClick={handleShowDialog} className={`${styles.imageSmall} fixed z-10 inset-0 overflow-y-auto ms-2 mt-2`}
                        src={i.url}
                        alt="no image" />
            ))}

                    {isOpen && (
                        <dialog
                            className={styles.dialog}
                            open
                            onClick={handleShowDialog}
                            scroll="paper" >
                            
                            <img className={styles.imageLarge}
                                src={imageToShow}
                                onClick={handleShowDialog}
                                alt="no image" />
                                
                        </dialog>
                    )}
            </div>
    )
}

export default Gallery;