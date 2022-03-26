export default function useCart(props) {
    const localCart = JSON.parse(window.localStorage.getItem('item-cart')) ?? [];

    const addItem = (item) => {
        const newCart = [...localCart, item];
        window.localStorage.setItem('item-cart', JSON.stringify(newCart));
    }

    const removeItem = (id) => {
        const newCart = localCart.filter(c => c.id !== id);
        window.localStorage.setItem('item-cart', JSON.stringify(newCart));
    }

    return [localCart, addItem, removeItem];
};