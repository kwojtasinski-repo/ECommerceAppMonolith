import useAuth from "./useAuth";
import axios from "../axios-setup";

export default function useCart(props) {
    const [auth] = useAuth();
    let cart = null;
    let addItem = (item) => {};
    let removeItem = (item) => {};

    if (auth) {
        [cart, addItem, removeItem] = authCart();
        const [localItemsCart] = localCart();

        if (localItemsCart.length > 0) {
            // add all to authCart
        }

    } else {
       [cart, addItem, removeItem] = localCart();
    }

    return [cart, addItem, removeItem];
};

function localCart() {
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
}

function authCart() {
    const cart = axios.get("/sales-module/cart/me");

    const addItem = (item) => {
        axios.post("/sales-module/order-items", item);
    }

    const removeItem = (id) => {
        axios.delete(`/sales-module/order-items/${id}`);
    }

    return [cart, addItem, removeItem];
}