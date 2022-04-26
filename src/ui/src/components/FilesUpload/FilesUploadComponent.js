import { useState } from "react";
import axios from "../../axios-setup";
import { mapToMessage } from "../../helpers/validation";
import LoadingButton from "../UI/LoadingButton/LoadingButton";

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

    const onSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        var formData = new FormData();
        for (const file of imageCollection.imgCollection) {
            formData.append('files', file);
        }

        try {
            const response = await axios.post(apiUrl, formData);
            props.urlImagesToReturn(response.data);
        } catch (exception) {
            debugger;
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

        setLoading(false);
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