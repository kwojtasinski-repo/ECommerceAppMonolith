import axios from "../../axios-setup";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

function Item(props) {
    const { id } = useParams();
    const [item, setItem] = useState(null);

    const fetchItem = async () => {
        const response = await axios.get(`/items-module/item-sales/${props.id}`);
        setItem(response.data);
    }

    useEffect(() => {
        fetchItem();
        console.log(item);
    }, []);

    return (
        <div>
            Item
        </div>
    )
}

export default Item;