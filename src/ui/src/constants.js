export const tokenData = 'token-data';

export const requestPath = {
    contactsModule: {
        addAddress: '/contacts-module/addresses',
        updateAddress: (id) => `/contacts-module/customers/${id}`,
        addCustomer: '/contacts-module/customers',
        updateCustomer: (id) => `/contacts-module/addresses/${id}`,
        getCustomer: (id) => `/contacts-module/customers/${id}`,
        getMyCustomers: '/contacts-module/customers/me',
        deleteCustomer: (id) => `/contacts-module/customers/${id}`
    },
    currenciesModule: {
        currencies: '/currencies-module/currencies',
        deleteCurrency: (id) => `/currencies-module/currencies/${id}`,
        refreshRates: '/currencies-module/currency-rates/refresh',
        getCurrency: (id) => `/currencies-module/currencies/${id}`,
        updateCurrency: (id) => `/currencies-module/currencies/${id}`,
        addCurrency: '/currencies-module/currencies',
        latestRates: 'currencies-module/currency-rates/latest'
    },
    itemsModule: {
        brands: '/items-module/brands',
        deleteBrand: (id) => `/items-module/brands/${id}`,
        getBrand: (id) => `/items-module/brands/${id}`,
        updateBrand: (id) => `/items-module/brands/${id}`,
        addBrand: '/items-module/brands',
        types: '/items-module/types',
        deleteType: (id) => `/items-module/types/${id}`,
        getType: (id) => `/items-module/types/${id}`,
        updateType: (id) => `/items-module/types/${id}`,
        addType: '/items-module/types',
        getItem: (id) => `/items-module/items/${id}`,
        updateItem: (id) => `/items-module/items/${id}`,
        deleteItem: (id) => `/items-module/items/${id}`,
        addItem: '/items-module/items',
        getItemsNotPutUpForSale: '/items-module/items/not-put-up-for-sale',
        itemsForSale: '/items-module/item-sales',
        getItemForSale: (id) => `/items-module/item-sales/${id}`,
        addItemForSale: '/items-module/item-sales',
        deleteItemForSale: (id) => `/items-module/item-sales/${id}`,
        searchItemForSale: (term) => `/items-module/item-sales/search?name=${term}`
    },
    salesModule: {
        getArchivePosition: (id) => `/sales-module/order-items/${id}`,
        acceptCart: 'sales-module/order-items/multi',
        getMyCart: 'sales-module/cart/me',
        removePositionFromCart: (id) => `sales-module/order-items/${id}`,
        getOrder: (id) => `/sales-module/orders/${id}`,
        getMyOrders: '/sales-module/orders/me',
        changeCustomerOnOrder: (id) => `/sales-module/orders/${id}/customer/change`,
        orders: '/sales-module/orders',
        addPayment: '/sales-module/payments',
        changeCurrencyOnOrder: (id) => `/sales-module/orders/${id}/currency/change`
    },
    purchaseProfilerModule: {
        recommendations: '/purchase-profiler-module/recomendations'
    },
    usersModule: {
        path: '/users-module',
        account: '/users-module/account',
        login: 'users-module/account/sign-in',
        register: 'users-module/account/sign-up',
        changeCredentials: 'users-module/account/change-credentials',
        accounts: '/users-module/accounts',
        userAccount: (id) => `/users-module/accounts/${id}`,
        updateUserPolicies: (id) => `/users-module/accounts/${id}/policies`,
        activeUser: (id) => `/users-module/accounts/${id}/active`
    }
};

