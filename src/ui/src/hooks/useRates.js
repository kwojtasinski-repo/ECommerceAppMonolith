import { useEffect, useState } from 'react';
import axios from '../axios-setup';
import { convertHourToLong, convertSecondsToLong } from '../helpers/dateHelper';
import { mapToCurrencyRates } from '../helpers/mapper';
import { getWithExpiry, setWithExpiry } from '../helpers/storage';

export default function useRates() {
    const [rates, setRates] = useState(getWithExpiry(key));

    const getRates = async () => {
        const response = await axios.get('currencies-module/currency-rates/latest');
        const ratesLocal = mapToCurrencyRates(response.data);
        debugger;
        const seconds = process.env.REACT_APP_EXPIRY_CURRENCY_RATES;
        const time = convertSecondsToLong(seconds);
        setWithExpiry(key, ratesLocal, time);
        setRates(ratesLocal);
    }

    useEffect(() => {
        if (rates === null) {
            getRates();
        }

    }, [rates])

    return rates;
}

const key = 'curreny-rates';