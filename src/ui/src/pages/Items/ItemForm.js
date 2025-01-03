import { useCallback, useEffect, useState } from "react";
import { NavLink } from "react-router";
import Input from "../../components/Input/Input";
import { Color } from "../../components/Notification/Notification";
import Popup, { Type } from "../../components/Popup/Popup";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { mapToMessage, validate } from "../../helpers/validation";
import useNotification from "../../hooks/useNotification";
import Tags from "../../components/Tags/Tags";
import PropTypes from 'prop-types';
import { isEmpty } from "../../helpers/stringExtensions";
import Images from "./Images/Images";
import AddImages from "./Images/AddImages";

function ItemForm(props) {
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [loadingImages, setLoadingImages] = useState(false);
    const [shareImages, setShareImages] = useState([]);
    const limitImages = Number(process.env.REACT_APP_ALLOWED_MAX_IMAGES);
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
    const addNotification = useNotification().addNotification;
    const [imageError, setImageError] = useState('');

    const changeHandler = useCallback((value, fieldName) => {
        setForm(prevFom => {
            const error = validate(prevFom[fieldName].rules, value);
            return {
                ...prevFom, 
                [fieldName]: {
                    ...prevFom[fieldName],
                    value,
                    showError: true,
                    error: error
                }
            };
        });
    }, []);

    const validateBeforeSend = (form, setForm) => {
        const errors = [];
        const formToUpdate = {...form};
        for(let field in form) {
            const error = validate(form[field].rules, form[field].value);
            if (!error) {
                continue;
            }

            formToUpdate[field] = {
                ...formToUpdate[field],
                showError: true,
                error
            };
            errors.push({ field, error});
        }

        if (!isEmpty(errors)) {
            setForm(formToUpdate);
        }

        return errors;
    }

    const clearErrors = (form) => {
        for(let field in form) {
            if (!form[field]?.showError || !form[field]?.error) {
                continue;
            }

            form[field] = {
                ...form[field],
                showError: false,
                error: ''
            };
        }

        return form;
    }

    const submit = async (event) => {
        event.preventDefault();
        setLoading(true);
        clearErrors(form);
        const itemErrors = validateBeforeSend(form, setForm);
        if (!isEmpty(itemErrors)) {
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
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }

            setError(errorMessage);
            setLoading(false);
        }        
    }

    const handleSendImages = () => {
        if (isEmpty(shareImages) || shareImages.find(i => !i.url)) {
            setImageError('Obrazki nie mogą mieć pustego adresu');
            return;
        }
        setImageError('');

        setIsOpen(false);
        const imagesToUpdate = [...form.imagesUrl.value, ...shareImages];
        changeHandler(imagesToUpdate, 'imagesUrl');
    }

    const handleClosePopup = () => {
        setIsOpen(false);
        setImageError('');
    }

    const handleAddImages = () => {
        if ((form.imagesUrl.value.length + 1) >= limitImages) {
            const notificationError = { color: Color.error, id: new Date().getTime(), text: `Przekroczono dozwolony limit (${limitImages})`, timeToClose: 5000 };
            addNotification(notificationError);
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
        if (!props.brands || props.brands.length === 0) {
            setTypes([]);
            changeHandler('', 'brandId');
            return;
        }

        setBrands(props.brands);
        changeHandler(props.brands[0].id, 'brandId');
    }, [props.brands, changeHandler]);

    useEffect(() => {
        if (!props.types || props.types.length === 0) {
            setTypes([]);
            changeHandler('', 'typeId');
            return;
        }

        setTypes(props.types);
        changeHandler(props.types[0].id, 'typeId');
    }, [props.types, changeHandler])

    useEffect(() => {
        if (props.item) {
            setForm(prevFom => ({
                ...prevFom,
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
                }}));
        }
    }, [props.item])

    return (
        <div>
            <h4>{props.text}</h4>
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            {isOpen && <Popup handleConfirm = {handleSendImages}
                                  textConfirm = "Ok"  
                                  handleClose = {handleClosePopup}// handleClosePopup
                                  popupType = "send"
                                  type = {Type.info}
                                  loading = {loadingImages}
                                  content = {<>
                                        <AddImages setShareImages = {setShareImages} limit = {limitImages} setLoadingImages = {setLoadingImages} imageError = {imageError} /> 
                                      </>}
                    /> }
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

                <Tags tags = {form.tags.value} setShareTags = {setShareTags} canEdit = {true} />

                <button type="button" className="btn btn-primary mt-2" onClick={handleAddImages}>
                    Dodaj obrazki
                </button>

                <div className="mt-2 mb-2">
                    <Images imagesUrl={form.imagesUrl.value} setMainImage={setMainImage} handleDeleteImage={handleDeleteImage}/>
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

export default ItemForm;

ItemForm.defaultProps = {
    editForm: true
};

ItemForm.propTypes = {
    brands: PropTypes.arrayOf(PropTypes.object),
    types: PropTypes.arrayOf(PropTypes.object),
    redirectAfterSuccess: PropTypes.func.isRequired
}
