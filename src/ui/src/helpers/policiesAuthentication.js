import React from "react";
import AddBrand from "../pages/Brands/Add/AddBrand";
import Brands from "../pages/Brands/Brands";
import EditBrand from "../pages/Brands/Edit/EditBrand";
import AddCurrency from "../pages/Currencies/AddCurrency/AddCurrency";
import Currencies from "../pages/Currencies/Currencies";
import EditCurrency from "../pages/Currencies/EditCurrency/EditCurrency";
import AddItem from "../pages/Items/AddItem/AddItem";
import EditItem from "../pages/Items/EditItem/EditItem";
import ItemDetails from "../pages/Items/ItemDetails/ItemDetails";
import Items from "../pages/Items/Items";
import ItemForSaleEdit from "../pages/Items/ItemsForSale/ItemForSaleEdit/ItemForSaleEdit";
import ItemsForSale from "../pages/Items/ItemsForSale/ItemsForSale";
import PutItemForSale from "../pages/Items/PutItemForSale/PutItemForSale";
import TypeAdd from "../pages/Types/Add/TypeAdd";
import TypeEdit from "../pages/Types/Edit/TypeEdit";
import Types from "../pages/Types/Types";
import EditUser from "../pages/Users/EditUser/EditUser";
import Users from "../pages/Users/Users";

export function policiesAuthentication(props) {
    let claims = [];

    React.Children.map(props, (child) => {
        switch(child.type) {
            case Brands : 
                claims = [ "items" ];
                break;
            case AddBrand : 
                claims = [ "items" ];
                break;
            case EditBrand : 
                claims = [ "items" ];
                break;
            case Items : 
                claims = [ "items" ];
                break;
            case AddItem : 
                claims = [ "items" ];
                break;
            case EditItem : 
                claims = [ "items" ];
                break;
            case ItemDetails : 
                claims = [ "items" ];
                break;
            case Types : 
                claims = [ "items" ];
                break;
            case TypeAdd : 
                claims = [ "items" ];
                break;
            case TypeEdit: 
                claims = [ "items" ];
                break;
            case ItemsForSale :
                claims = [ "items", "item-sale" ];
                break;
            case PutItemForSale :
                claims = [ "items", "item-sale" ];
                break;
            case ItemForSaleEdit:
                claims = [ "items", "item-sale" ];
                break;
            case Currencies :
                claims = [ "currencies" ];
                break;
            case EditCurrency :
                claims = [ "currencies" ];
                break;
            case AddCurrency :
                claims = [ "currencies" ];
                break;
            case Users:
                claims = [ "users" ];
                break;
            case EditUser:
                claims = [ "users" ];
                break;
            default:
                break;
        }
    });
    
    return claims;
}