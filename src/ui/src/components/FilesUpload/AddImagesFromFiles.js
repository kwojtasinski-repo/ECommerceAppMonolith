import { useEffect, useState } from "react";
import FilesUploadComponent from "./FilesUploadComponent";

export default function AddImagesFromFiles(props) {
    const [images, setImages] = useState([]);

    const fetchUrls = (urls) => {
        const urlsToSet = [];
        for(const url of urls) {
            const baseUrl = window._env_?.REACT_APP_BACKEND_URL ? window._env_.REACT_APP_BACKEND_URL : process.env.REACT_APP_BACKEND_URL;
            const urlToImages = baseUrl + url;
            urlsToSet.push({
                url: urlToImages,
                mainImage: false
            });
        }

        setImages(urlsToSet);
    }
    
    useEffect(()=> {
        if (!props.setShareImages) {
            return;
        }

        props.setShareImages([
            ...images
        ])
    }, [images, props]);

    return (
        <div>
            <FilesUploadComponent apiUrl = '/items-module/images'
                                  limit = {props.limit}
                                  urlImagesToReturn = {fetchUrls}
                                  setLoadingImages = {props.setLoadingImages} />
        </div>
    )
}