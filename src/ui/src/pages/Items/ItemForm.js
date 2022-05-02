import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import Input from "../../components/Input/Input";
import { Color } from "../../components/Notification/Notification";
import Popup, { Type } from "../../components/Popup/Popup";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { mapToMessage, validate } from "../../helpers/validation";
import useNotification from "../../hooks/useNotification";
import style from "./ItemForm.module.css";
import FilesUploadComponent from "../../components/FilesUpload/FilesUploadComponent";
import Tags from "../../components/Tags/Tags";
import PropTypes from 'prop-types';
import { isEmpty } from "../../helpers/stringExtensions";

function ItemForm(props) {
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [loadingImages, setLoadingImages] = useState(false);
    const [imageAddSource, setImageAddSource] = useState('');
    const [shareImages, setShareImages] = useState([]);
    const [limitImages, setLimitImages] = useState(process.env.REACT_APP_ALLOWED_MAX_IMAGES);
    const [brands, setBrands] = useState(props.brands);
    const [types, setTypes] = useState(props.types);
    const [error, setError] = useState('');
    const [form, setForm] = useState({
        id: {
            value: ''
        },
        itemName: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
        },
        description: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'min', length: 3 }]
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
            value: [],
            error: '',
            showError: false,
            rules: [] // optional
        },
        imagesUrl: {
            value: [],
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

    const validateBeforeSend = (form, setForm) => {
        for(let field in form) {
            const error = validate(form[field].rules, form[field].value);

            if (error) {
                setForm({...form, 
                    [field]: {
                        ...form[field],
                        showError: true,
                        error
                    }});
                setLoading(false);
                return error;
            }
        }
    }

    const submit = async (event) => {
        event.preventDefault();
        setLoading(true);
        const errorItem = validateBeforeSend(form, setForm);
        if (!isEmpty(errorItem)) {
            setLoading(false);
            return;
        }

        try {
            await props.onSubmit({
                itemId: form.id.value,
                itemName: form.itemName.value,
                description: form.description.value,
                brandId: form.brandId.value,
                typeId: form.typeId.value,
                tags: form.tags.value,
                imagesUrl: form.imagesUrl.value
            });
            setLoading(false);
            props.redirectAfterSuccess();
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status);
            }

            setError(errorMessage);
            setLoading(false);
        }        
    }

    const handleSendImages = () => {
        setLoadingImages(true);
        setLoadingImages(false);
        setIsOpen(false);
        setImageAddSource('');
        const imagesToUpdate = [...form.imagesUrl.value, ...shareImages];
        changeHandler(imagesToUpdate, 'imagesUrl');
        setLimitImages(limitImages - shareImages.length);
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

    const setShareTags = (tags) => {
        changeHandler(tags, 'tags');
    }

    const handleDeleteImage = (value) => {
        const imagesDeleted = form.imagesUrl.value.filter(i => i !== value);
        changeHandler(imagesDeleted, 'imagesUrl');
        setLimitImages(limitImages + 1);
    }

    const setMainImage = (image) => {
        let mainImage = form.imagesUrl.value.find(i => i.mainImage);
        let sameImage = false;
        if (mainImage) {
            mainImage.mainImage = false;
            sameImage = mainImage.url === image.url;
        }

        if (sameImage) {
            changeHandler(form.imagesUrl.value, 'imagesUrl');
            return;    
        }

        let imageFound = form.imagesUrl.value.find(i => i.url === image.url);
        imageFound.mainImage = true;
        changeHandler(form.imagesUrl.value, 'imagesUrl');
    }

    useEffect(() => {
        const brand = props.brands.find(b => true);
        setBrands(props.brands);

        if (brand) {
            changeHandler(brand.id, 'brandId');
        }
    }, [props.brands]);

    useEffect(() => {
        const type = props.types.find(t => true);
        setTypes(props.types);
        
        if (type) {
            changeHandler(type.id, 'typeId');
        }
    }, [props.types])

    useEffect(() => {
        if (props.item) {
            setForm({
                ...form,
                id: {
                    value: props.item.id
                },
                itemName: {
                    value: props.item.name
                },
                description: {
                    value: props.item.description
                },
                brandId: {
                    value: props.item.brandId
                },
                typeId: {
                    value: props.item.typeId
                },
                tags: {
                    value: props.item.tags
                },
                imagesUrl: {
                    value: props.item.imagesUrl
                }})
        }
    }, [props.item])

    return (
        <div>
            <h4>{props.text}</h4>
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
                       error = {form.brandId.error}
                       showError = {form.brandId.showError}
                       options = { brands.map(b => {
                           return {
                               key: b.id,
                               value: b.id,
                               label: b.name
                           }
                       })} 
                       onChange = {val => changeHandler(val, 'brandId')} />

                <Input label = "Typ"
                       type = "select"
                       error = {form.typeId.error}
                       showError = {form.typeId.showError}
                       options = { types.map(t => {
                           return {
                               key: t.id,
                               value: t.id,
                               label: t.name
                           }
                       })} 
                       onChange = {val => changeHandler(val, 'typeId')} />

                <Tags tags = {form.tags.value} setShareTags = {setShareTags} />

                <button type="button" className="btn btn-primary mt-2" onClick={handleAddImages}>
                    Dodaj obrazki
                </button>

                {isOpen && <Popup handleConfirm = {handleSendImages}
                                  textConfirm = "Ok"  
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
                    {form.imagesUrl.value.map(i => (
                        <div key={new Date().getTime() + Math.random()}
                             style={i.mainImage ? { display: "inline-block", border: "3px solid rgb(25, 135, 84)" } : { display: "inline-block" }}
                             className="me-2">
                            <img key={new Date().getTime() + (Math.random() * (99999 - 1) + 1)}
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
                    <LoadingButton
                        loading={loading} 
                        className="btn btn-success">
                        {props.buttonText}
                    </LoadingButton>
                    <NavLink className="btn btn-secondary ms-2" to = {props.cancelEditUrl} >{props.cancelButtonText}</NavLink>
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
    const [images, setImages] = useState([]);

    const fetchUrls = (urls) => {
        const urlsToSet = [];
        for(const url of urls) {
            urlsToSet.push({
                url,
                mainImage: false
            });
        }

        setImages(urlsToSet);
    }
    
    useEffect(()=> {
        props.setShareImages([
            ...images
        ])
    }, [images]);

    return (
        <div>
            <FilesUploadComponent apiUrl = '/items-module/images'
                                  limit = {props.limit}
                                  urlImagesToReturn = {fetchUrls} />
        </div>
    )
}

export default ItemForm;

ItemForm.defaultProps = {
    editForm: true
};

ItemForm.propTypes = {
    brands: PropTypes.arrayOf(PropTypes.object),
    types: PropTypes.arrayOf(PropTypes.object),
    redirectAfterSuccess: PropTypes.func.isRequired
}