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
        props.setShareImages([{
            ...images.image
        }])
    }, [images]);

    return (
        <div>
            <Input label = "Podaj adres"
                   onChange = {val => changeHandler(val, 'image')}
                   value = {images.image.url} />
        </div>
    )
}