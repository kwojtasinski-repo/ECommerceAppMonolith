import { useEffect, useState } from "react"
import style from "./Image.module.css";

export default function Image(props) {
    const [image, setImage] = useState(null);

    useEffect(() => {
        if (props.image) {
            setImage(props.image);            
        }
    }, [props.image])

    return (
        <>
            { image === null ? <></>
                :
                <div style={image.mainImage ? { display: "inline-block", border: "3px solid rgb(25, 135, 84)" } : { display: "inline-block" }}
                     className="me-2">
                    <img key={new Date().getTime() + (Math.random() * (99999 - 1) + 1)}
                        className = {style.imageSmall}
                        src={image.url}
                        alt="imgSmall"
                        onClick={() => props.setMainImage(image)}/>
                    <button key={new Date().getTime() + Math.random()} 
                            type="button" 
                            className={`${style.xButton}`}
                            onClick = {() => props.deleteImage(image)}>
                                X
                    </button>
                </div>
            }
        </>
    )
}