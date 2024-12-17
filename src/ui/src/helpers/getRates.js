import axios from "../axios-setup";
import { convertSecondsToLong } from "./dateHelper";
import { mapToCurrencyRates } from "./mapper";
import { getWithExpiry, setWithExpiry } from "./storage";

export async function getRates (refresh) {
    let rates = getWithExpiry(key)

    if (!rates || refresh) {
        const response = await axios.get('currencies-module/currency-rates/latest');
        rates = mapToCurrencyRates(response.data);
        const seconds = process.env.REACT_APP_EXPIRY_CURRENCY_RATES;
        const time = convertSecondsToLong(seconds);
        setWithExpiry(key, rates, time);
    }

    return rates;
}

const key = 'curreny-rates';