export const mapToItems = obj => {
    const newItems = [];
    for (const key in obj) {
        const item = {
            id: obj[key].id,
            name: obj[key].item.itemName,
            cost: obj[key].cost,
            code: obj[key].currencyCode,
            imageUrl: obj[key].item.imageUrl.url
        }
        newItems.push(item);
    }

    return newItems;
}

export const mapToItem = obj => {
    const item = {
        id: obj.id,
        name: obj.item.itemName,
        description: obj.item.description,
        cost: obj.cost,
        tags: obj.item.tags,
        brandId: obj.item.brand.id,
        brand: obj.item.brand.name,
        typeId: obj.item.type.id,
        type: obj.item.type.name,
        code: obj.currencyCode,
        imagesUrl: obj.item.imagesUrl
    }

    return item;
}

export const mapToCurrencies = (obj) => {
    const newCurrencies = [];
    for (const key in obj) {
        const item = {
            id: obj[key].id,
            code: obj[key].code,
            description: obj[key].description
        }
        newCurrencies.push(item);
    }

    return newCurrencies;
};

export const mapToCurrency = (obj) => {
    const currency = {
        id: obj.id,
        code: obj.code,
        description: obj.description
    }

    return currency;
};

export const mapToCurrencyRates = (obj) => {
    const newCurrencies = [];
    for (const key in obj) {
        const item = {
            id: obj[key].id,
            code: obj[key].code,
            currencyId: obj[key].currencyId,
            rate: obj[key].rate,
            currencyDate: obj[key].currencyDate
        }
        newCurrencies.push(item);
    }

    return newCurrencies;
};