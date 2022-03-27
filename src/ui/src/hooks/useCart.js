export default function useCart(props) {
    let cart = null;
    let addItem = (item) => {};
    let removeItem = (item) => {};
    [cart, addItem, removeItem] = localCart();
    return [cart, addItem, removeItem];
};

function localCart() {
    let cart = JSON.parse(window.localStorage.getItem('item-cart')) ?? [];

    const addItem = (item) => {
        console.log('addItem local');
        debugger;
        const itemExists = cart.find(i => i.id === item.id);
        let newItem = {...item, quantity: 1 };
        let newCart = [];
        
        if (itemExists) {
            newItem.quantity = itemExists.quantity + 1;
            newCart = cart.filter(c => c.id !== newItem.id);
            newCart = [...newCart, newItem];
        } else {
            newCart = [...cart, newItem];
        }

        window.localStorage.setItem('item-cart', JSON.stringify(newCart));
    }

    const removeItem = (id) => {
        const itemExists = cart.find(i => i.id === id);
        let newCart = [...cart];

        if (itemExists) {
            itemExists.quantity = itemExists.quantity - 1;
            newCart = cart.filter(c => c.id !== id);
            newCart = [...newCart, itemExists];
        } else {
            newCart = cart.filter(c => c.id !== id);
        }

        window.localStorage.setItem('item-cart', JSON.stringify(newCart));
    }

    return [cart, addItem, removeItem];
}