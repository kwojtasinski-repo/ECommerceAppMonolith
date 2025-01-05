import { useCallback, useEffect, useState } from "react";
import { mapToItems } from "../../helpers/mapper";
import axios from '../../axios-setup';
import Items from "../../components/Items/Items";
import LoadingIcon from "../../components/UI/LoadingIcon/LoadingIcon";
import useWebsiteTitle from "../../hooks/useWebsiteTitle";
import { calculateItems } from "../../helpers/calculationCost";
import { getRates } from "../../helpers/getRates";
import { mapToMessage } from "../../helpers/validation";
import { requestPath } from "../../constants";

function Home() {
    const [items, setItems] = useState([]);
    const [loading, setLoading] = useState(true);
    const setTitle = useWebsiteTitle();
    const [error, setError] = useState('');
    const homeTabName = process.env.REACT_APP_HOME_TAB_NAME;

    const fetchItems = useCallback(async () => {
        try {
            const rates = await getRates();
            const response = await axios.get(requestPath.itemsModule.itemsForSale);
            const items = mapToItems(response.data);
            calculateItems(items, rates);
            setItems(items);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            errorMessage += mapToMessage(errors, status) + '\n';
            setError(errorMessage);
        }
        setLoading(false);
        setTitle(homeTabName);
    }, [homeTabName, setTitle]);
  
    useEffect(() => {
        fetchItems();
    }, [fetchItems]);

    return loading ? <LoadingIcon /> : (
        <>
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            <Items items={items} />
        </>
    )
}

export default Home;