import { useEffect, useState } from "react";
import Image from "./Image";

export default function Images(props) {
    const [images, setImages] = useState([]);

    const markAsMainImage = (image) => {
        props.setMainImage(image);
    }
    
    const deleteImage = (value) => {
        props.handleDeleteImage(value);
    }

    useEffect(() =>{
        if (props.imagesUrl) {
            setImages(props.imagesUrl);
        }
    }, [props.imagesUrl])

    return (
        <>
            {images.map(i => (
                <Image key={new Date().getTime() + Math.random()} image={i} setMainImage={markAsMainImage}  deleteImage={deleteImage} />
            ))}
        </>
    )
}
