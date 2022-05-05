export function getSampleOrderItems() {
    const orderItems = [];

    for(let i = 0; i < 3; i++) {
        const orderItem = {
            id: i+1,
            itemCartId: i+1,
            itemCart: {
                itemName: 'Item #' + (i+1),
                brandName: 'Brand #' + (i+1),
                typeName: 'Type #' + (i+1),
                description: 'Description item #' + (i+1),
                tags: [],
                imagesUrl: [''],
                cost: 100 * (i+1),
                created: new Date(),
                currencyCode: 'PLN'
            },
            code: 'PLN',
            rate: 1,
            cost: 100 * (i+1),
            userId: 1
        };
        orderItems.push(orderItem);
    }

    return orderItems;
}

export function getSampleBrands() {
    const brands = [];

    for(let i = 0; i < 3; i++) {
        const brand = {
            id: (i+1),
            name: "Brand #" + (i+1)
        };
        brands.push(brand);
    }

    return brands;
}