import axios from "../../../axios-setup";
import { useEffect, useState } from "react";
import ItemForm from "../ItemForm";
import { mapToBrands, mapToTypes } from "../../../helpers/mapper";
import { useNavigate } from "react-router";
import { requestPath } from "../../../constants";

function AddItem() {
    const [brands, setBrands] = useState([]);
    const [types, setTypes] = useState([]);
    let id = '';
    const navigate = useNavigate();

    const onSubmit = async (form) => {
        const response = await axios.post(requestPath.itemsModule.addItem, form);
        // TODO: I think it is better to also add the itemId as payload
        const itemId = response.headers.location.split('/items-module/items/')[1];
        id = itemId;
    }

    const redirectAfterSuccess = () => {
        navigate(`/items/details/${id}`);
    }

    const fetchBrands = async () => {
        const response = await axios.get(requestPath.itemsModule.brands);
        setBrands(mapToBrands(response.data));
    }

    const fetchTypes = async () => {
        const response = await axios.get(requestPath.itemsModule.types);
        setTypes(mapToTypes(response.data));
    }

    useEffect(() => {
        fetchBrands();
        fetchTypes();
    }, []);

    return (
        <div>
            <ItemForm text = "Dodaj przedmiot"
                      onSubmit = {onSubmit}
                      cancelEditUrl = "/items"
                      cancelButtonText = "Anuluj"
                      buttonText = "Dodaj"
                      brands = {brands}
                      types = {types}
                      redirectAfterSuccess = {redirectAfterSuccess} />
        </div>
    )
}

export default AddItem;