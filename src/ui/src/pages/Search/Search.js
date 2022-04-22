import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Items from "../../components/Items/Items";
import axios from "../../axios-setup";
import { mapToItems } from "../../helpers/mapper";
import { mapToMessage } from "../../helpers/validation";

function Search(props) {
    const { term } = useParams();
    const [items, setItems] = useState([]);
    const [error, setError] = useState('');

    const searchHandler = async () => {
        try {
            const response = await axios.get(`/items-module/item-sales/search?name=${term}`);
            setItems(mapToItems(response.data));
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status);
            setError(errorMessage);
        }
    }

    useEffect(() => {
        searchHandler();
    }, [term])

    return (
        <div>
            <h2>Wyniki wyszukiwania "{term}"</h2>
            {error ? (
                <div className="alert alert-danger">
                    {error}
                </div>
            ) : null}
            <Items items={items} />
        </div>
    )
}

export default Search;