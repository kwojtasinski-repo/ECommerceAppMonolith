import React from "react";
import AddItem from "../pages/Items/AddItem/AddItem";
import DeleteItem from "../pages/Items/DeleteItem/DeleteItem";
import EditItem from "../pages/Items/EditItem/EditItem";
import Items from "../pages/Items/Items";
import PutItemForSale from "../pages/Items/PutItemForSale/PutItemForSale";

export function policiesAuthentication(props) {
    let claims = [];

    React.Children.map(props, (child) => {
        switch(child.type) {
            case Items : 
                claims = [ "items" ];
                break;
            case AddItem :
                claims = [ "items" ];
                break;
            case EditItem :
                claims = [ "items" ];
                break;
            case DeleteItem :
                claims = [ "items" ];
                break;
            case PutItemForSale:
                claims = [ "items", "item-sale" ];
                break;
            default:
                break;
        }
    });
    
    return claims;
}