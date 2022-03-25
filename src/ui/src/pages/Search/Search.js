import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Items from "../../components/Items/Items";
import axios from "../../axios-setup";
import { mapToItems } from "../../helpers/mapper";

function Search(props) {
    const { term } = useParams();
    const [items, setItems] = useState([]);

    const searchHandler = async () => {
        const response = await axios.get(`/items-module/item-sales/search?name=${term}`);
        setItems(mapToItems(response.data));
    }

    useEffect(() => {
        searchHandler();
    }, [term])

    return (
        <div>
            <h2>Wyniki wyszukiwania "{term}"</h2>
            <Items items={items} />
        </div>
    )
}

export default Search;