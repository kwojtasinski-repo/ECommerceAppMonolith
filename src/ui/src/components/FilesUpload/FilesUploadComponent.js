import { useState } from "react";
import axios from "../../axios-setup";
import { mapToMessage } from "../../helpers/validation";
import LoadingButton from "../UI/LoadingButton/LoadingButton";
import PropTypes from 'prop-types';

function FilesUploadComponent(props) {
    const limit = props.limit;
    const apiUrl = props.apiUrl;
    const [loading, setLoading] = useState(false);
    const [buttonDisabled, setButtonDisabled] = useState(false);
    const [error, setError] = useState('');
    const [imageCollection, setImageCollection] = useState('');

    const onFileChange = (e) => {
        setButtonDisabled(false);
        if (limit && limit < e.target.files.length) {
            alert(`Maksymalna ilość plików do przesłania ${limit}`);
            setButtonDisabled(true);
            return;
        }
        setImageCollection({ imgCollection: e.target.files });
    }

    const setLoadingImages = (loading) => {
        if (props.setLoadingImages) {
            props.setLoadingImages(loading);
        }
    }

    const onSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
        setLoadingImages(true);
        
        if (!imageCollection.imgCollection) {
            setError('Nie wybrano plików do wysłania');
            setLoading(false);
            setLoadingImages(false);
            return;
        }

        var formData = new FormData();
        for (const file of imageCollection.imgCollection) {
            formData.append('files', file);
        }

        try {
            const response = await axios.post(apiUrl, formData);
            props.urlImagesToReturn(response.data);
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
            setLoadingImages(false);
        }

        setLoading(false);
        setLoadingImages(false);
    }

    return (
        <div className="container">
            <div className="row">
                {error ? (
                    <div className="alert alert-danger">{error}</div>
                ) : null}
                <form onSubmit={onSubmit}>
                    <div className="form-group">
                        <input type="file" 
                               name="files"
                               onChange={onFileChange} multiple />
                    </div>
                    <div className="form-group">
                        <LoadingButton className="btn btn-primary mt-2" 
                                       type="button"
                                       onClick={onSubmit}
                                       loading={loading}
                                       disabled={buttonDisabled}>
                                           Wgraj
                        </LoadingButton>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default FilesUploadComponent;

FilesUploadComponent.propTypes = {
    apiUrl: PropTypes.string.isRequired,
    urlImagesToReturn: PropTypes.func.isRequired
}