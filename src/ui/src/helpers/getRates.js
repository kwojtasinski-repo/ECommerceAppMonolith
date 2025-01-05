import axios from "../axios-setup";
import { requestPath } from "../constants";
import { convertSecondsToLong } from "./dateHelper";
import { mapToCurrencyRates } from "./mapper";
import { getWithExpiry, setWithExpiry } from "./storage";

export async function getRates (refresh) {
    let rates = getWithExpiry(currencyRateKey)

    if (!rates || refresh) {
        const response = await axios.get(requestPath.currenciesModule.latestRates);
        rates = mapToCurrencyRates(response.data);
        const seconds = process.env.REACT_APP_EXPIRY_CURRENCY_RATES;
        const time = convertSecondsToLong(seconds);
        setWithExpiry(currencyRateKey, rates, time);
    }

    return rates;
}

export const currencyRateKey = 'curreny-rates';
