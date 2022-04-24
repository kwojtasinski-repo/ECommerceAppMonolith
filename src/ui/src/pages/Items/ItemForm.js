import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import Input from "../../components/Input/Input";
import { Color } from "../../components/Notification/Notification";
import Popup, { Type } from "../../components/Popup/Popup";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { validate } from "../../helpers/validation";
import useNotification from "../../hooks/useNotification";
import style from "./ItemForm.module.css";

function ItemForm(props) {
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [loadingImages, setLoadingImages] = useState(false);
    const [imageAddSource, setImageAddSource] = useState('');
    const [shareImages, setShareImages] = useState([]);
    const [limitImages, setLimitImages] = useState(process.env.REACT_APP_ALLOWED_MAX_IMAGES);
    const [images, setImages] = useState([]);
    const [error, setError] = useState('');
    const [form, setForm] = useState({
        id: {
            value: ''
        },
        itemName: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 3 }]
        },
        description: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 3 }]
        },
        brandId: {
            value: '',
            error: '',
            showError: false,
            rules: ['required']
        },
        typeId: {
            value: '',
            error: '',
            showError: false,
            rules: ['required']
        },
        tags: {
            value: '',
            error: '',
            showError: false,
            rules: [] // optional
        },
        imagesUrl: {
            value: '',
            error: '',
            showError: false,
            rules: [] // optional
        },
    });
    const notification = useNotification();

    const changeHandler = (value, fieldName) => {
        const error = validate(form[fieldName].rules, value);
        setForm({
            ...form, 
            [fieldName]: {
                ...form[fieldName],
                value,
                showError: true,
                error: error
            } 
        });
    };

    const submit = () => {

    }

    const handleSendImages = () => {
        console.log('limit obrazkow', limitImages);
        setLoadingImages(true);
        setTimeout(() => {
            setLoadingImages(false);
            setIsOpen(false);
            setImageAddSource('');
            console.log(shareImages);
            setImages([
                ...images,
                ...shareImages
            ]);
            setLimitImages(limitImages - shareImages.length);
        }, 1000);
    }

    const handleCloseSendImages = () => {
        setIsOpen(false);
        setImageAddSource('');
    }

    const handleAddImages = () => {
        if (limitImages <= 0) {
            const notificationError = { color: Color.error, id: new Date().getTime(), text: `Przekroczono dozwolony limit (${process.env.REACT_APP_ALLOWED_MAX_IMAGES})`, timeToClose: 5000 };
            notification[1](notificationError);
        } else {
            setIsOpen(true);
        }
    }

    const handleDeleteImage = (value) => {
        setImages(images.filter(i => i !== value));
        setLimitImages(limitImages + 1);
    }

    const setMainImage = (image) => {
        let mainImage = images.find(i => i.mainImage);
        if (mainImage) {
            mainImage.mainImage = false;
        }
        let imageFound = images.find(i => i.url === image.url);
        imageFound.mainImage = true;
        setImages([
            ...images
        ]);
    }

    return (
        <div>
            <h4>Dodaj przedmiot</h4>
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            <form onSubmit={submit} >
                <Input label = "Nazwa przedmiotu"
                        type = "text"
                        value = {form.itemName.value}
                        error = {form.itemName.error}
                        showError = {form.itemName.showError}
                        onChange = {val => changeHandler(val, 'itemName')} />

                <Input label = "Opis"
                        type = "textarea"
                        value = {form.description.value}
                        error = {form.description.error}
                        showError = {form.description.showError}
                        onChange = {val => changeHandler(val, 'description')} />
                    
                <Input label = "Firma"
                       type = "select" 
                       options = { [{key: 1, value: '1', label: 'Firma #1'}, {key: 2, value: '2', label: 'Firma #2'}]} 
                       onChange = {val => changeHandler(val, 'brandId')} />

                <Input label = "Typ"
                       type = "select"
                       options = { [{key: 1, value: '1', label: 'Typ #1'}, {key: 2, value: '2', label: 'Typ #2'}]} 
                       onChange = {val => changeHandler(val, 'typeId')} />

                <button type="button" className="btn btn-primary mt-2" onClick={handleAddImages}>
                    Dodaj obrazki
                </button>

                {isOpen && <Popup handleConfirm = {handleSendImages}
                                  textConfirm = "Wyślij"  
                                  handleClose = {handleCloseSendImages}
                                  popupType = "send"
                                  type = {Type.info}
                                  loading = {loadingImages}
                                  content = {<>
                                        <div className="mt-2 mb-2">
                                            {!imageAddSource ? (
                                                <>
                                                <p>Wybierz jak chcesz dodawać obrazki:</p>
                                                <div className="mb-2">
                                                    <button type="button" 
                                                            className="btn btn-primary"
                                                            onClick={() => setImageAddSource('files')}>
                                                                Z pliku
                                                    </button>
                                                </div>
                                                <div className="mb-2">
                                                    <button type="button"
                                                            className="btn btn-primary"
                                                            onClick={() => setImageAddSource('addresses')} >
                                                        Z adresu
                                                    </button>
                                                </div>
                                                </>
                                            ) : (imageAddSource === 'addresses' ? 
                                                        <AddImagesFromAddresses setShareImages = {setShareImages}/> : 
                                                        <AddImagesFromFiles limit = {limitImages} setShareImages = {setShareImages} />) }
                                        </div>
                                      </>}
                    /> }

                <div className="mt-2 mb-2">
                    {images.map(i => (
                        <div key={new Date().getTime() + Math.random()}
                             style={i.mainImage ? { display: "inline-block", border: "3px solid rgb(25, 135, 84)" } : { display: "inline-block" }}
                             className="me-2">
                            <img key={new Date().getTime() + Math.random()}
                                className = {style.imageSmall}
                                src={i.url}
                                alt="imgSmall"
                                onClick={() => setMainImage(i)}/>
                            <button key={new Date().getTime() + Math.random()} 
                                    type="button" 
                                    className={`${style.xButton}`}
                                    onClick = {() => handleDeleteImage(i)}>
                                        X
                            </button>
                        </div>
                    ))}
                    <br style={{clear:"both"}}/>
                </div>

                <div className="text-end mt-2">
                    <NavLink className="btn btn-secondary me-2" to = {props.cancelEditUrl} >{props.cancelButtonText}</NavLink>
                    <LoadingButton
                        loading={loading} 
                        className="btn btn-success">
                        {props.buttonText}
                    </LoadingButton>
                </div>
            </form>
        </div>
    )   
}

function AddImagesFromAddresses(props) {
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

function AddImagesFromFiles(props) {
    const limit = props.limit;

    return (
        <div>
            imagesFrom files and limit {limit}
        </div>
    )
}

export default ItemForm;