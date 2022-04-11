import { useEffect, useState } from "react";
import { mapToItems } from "../../helpers/mapper";
import axios from '../../axios-setup';
import Items from "../../components/Items/Items";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import useWebsiteTitle from "../../hooks/useWebsiteTitle";
import useRates from "../../hooks/useRates";
import { calculateItems } from "../../helpers/calculationCost";
import { getRates } from "../../helpers/getRates";

function Home(props) {
    const [items, setItems] = useState([]);
    const [loading, setLoading] = useState(true);
    const setTitle = useWebsiteTitle();

    const fetchItems = async () => {
        const rates = await getRates();
        const response = await axios.get(`/items-module/item-sales`);
        const items = mapToItems(response.data);
        calculateItems(items, rates);
        setItems(items);
        setLoading(false);
        setTitle('ECommerceApp');
    };

  
    useEffect(() => {
        fetchItems();
    }, []);

    return loading ? <LoadingIcon /> : (
        <>
            <Items items={items} />
        </>
    )
}

export default Home;