import { useEffect, useState } from "react"
import AddImagesFromAddresses from "../../../components/FilesUpload/AddImagesFromAddress";
import AddImagesFromFiles from "../../../components/FilesUpload/AddImagesFromFiles";

export default function AddImages(props) {
    const [imageAddSource, setImageAddSource] = useState(null);

    useEffect(() => {
        if (props.imageAddSource) {
            setImageAddSource(props.imageAddSource);
        }
    }, [props.imageAddSource])

    return (
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
                        <AddImagesFromAddresses setShareImages = {props.setShareImages}/> : 
                        <AddImagesFromFiles limit = {props.limitImages} setShareImages = {props.setShareImages} setLoadingImages = {props.setLoadingImages} />) }
        </div>
    )   
}