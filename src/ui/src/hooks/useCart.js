export default function useCart(props) {
    let cart = [];
    let addItem = (item) => {};
    let removeItem = (item) => {};
    let clear = () => {};
    [cart, addItem, removeItem, clear] = localCart();
    return {
        cart,
        addItem,
        removeItem,
        clear
    };
};

const key = 'item-cart';

function localCart() {
    let cart = JSON.parse(window.localStorage.getItem(key)) ?? [];

    const addItem = (item) => {
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

        window.localStorage.setItem(key, JSON.stringify(newCart));
    }

    const removeItem = (id) => {
        const itemExists = cart.find(i => i.id === id);
        
        if (itemExists) {
            let newCart = [...cart];
            itemExists.quantity = itemExists.quantity - 1;
            
            if (itemExists.quantity > 0) {
                newCart = cart.filter(c => c.id !== id);
                newCart = [...newCart, itemExists];
            } else {
                newCart = cart.filter(c => c.id !== id);
            }

            window.localStorage.setItem(key, JSON.stringify(newCart));
        }
    }

    const clear = () => {
        window.localStorage.removeItem(key);
    }

    return [cart, addItem, removeItem, clear];
}