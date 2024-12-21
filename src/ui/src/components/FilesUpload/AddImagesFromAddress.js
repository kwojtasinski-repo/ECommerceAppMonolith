import { useEffect, useState } from "react";
import Input from "../Input/Input";

export default function AddImagesFromAddresses(props) {
    const [images, setImages] = useState({
        image: {
            url: '',
            mainImage: false
        }
    })

    const changeHandler = (value, fieldName) => {
        setImages({
            ...images, 
            [fieldName]: {
                ...images[fieldName],
                url: value
            } 
        });
    };

    useEffect(()=> {
        if (!props.setShareImages) {
            return;
        }

        if (!images.image.url) {
            return;
        }

        props.setShareImages([{
            ...images.image
        }])
    }, [images, props]);

    return (
        <div>
            <Input label = "Podaj adres"
                   onChange = {val => changeHandler(val, 'image')}
                   value = {images.image.url} />
        </div>
    )
}