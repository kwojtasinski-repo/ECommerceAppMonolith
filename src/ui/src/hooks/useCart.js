import useAuth from "./useAuth";
import axios from "../axios-setup";

export default function useCart(props) {
    const [auth] = useAuth();
    let cart = null;
    let addItem = (item) => {};
    let removeItem = (item) => {};
    console.log('useCart');

    if (auth) {
        [cart, addItem, removeItem] = authCart();
        const [localItemsCart] = localCart();

        if (localItemsCart.length > 0) {
            // add all to authCart
        }

    } else {
       [cart, addItem, removeItem] = localCart();
    }

    console.log(cart);
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