import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import ItemForm from "../ItemForm";
import { mapToBrands, mapToTypes } from "../../../helpers/mapper";

function AddItem(props) {
    const [brands, setBrands] = useState([]);
    const [types, setTypes] = useState([]);

    const onSubmit = async (form) => {
        debugger;
        await axios.post('/items-module/items', form);
    }

    const fetchBrands = async () => {
        const response = await axios.get('/items-module/brands');
        setBrands(mapToBrands(response.data));
    }

    const fetchTypes = async () => {
        const response = await axios.get('/items-module/types');
        setTypes(mapToTypes(response.data));
    }

    useEffect(() => {
        fetchBrands();
        fetchTypes();
    }, []);

    return (
        <div>
            <ItemForm onSubmit = {onSubmit}
                      cancelEditUrl = "/items"
                      cancelButtonText = "Anuluj"
                      buttonText = "Dodaj"
                      brands={brands}
                      types={types} />
        </div>
    )
}

export default AddItem;