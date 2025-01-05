import { useEffect, useState } from 'react';
import { getWithExpiry } from '../helpers/storage';
import { currencyRateKey, getRates } from '../helpers/getRates';

export default function useRates() {
    const [rates, setRates] = useState(getWithExpiry(currencyRateKey));

    useEffect(() => {
        if (rates === null) {
            setRates(getRates());
        }

    }, [rates])

    return rates;
}
