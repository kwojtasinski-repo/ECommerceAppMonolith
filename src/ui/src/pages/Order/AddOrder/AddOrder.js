import { useState } from "react";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import Contacts from "../Contact/Contacts";

function AddOrder(props) {
    const [customer, setCustomer] = useState(null);
    const disabledButton = customer === null;

    const submit = async (form) => {
    }
    
    return (
        <div>
            <div>
                <h5>
                    {customer !== null ? `Wybrano: ${customer.firstName} ${customer.lastName} ${customer.companyName ? customer.companyName : ""}` : "Wybierz kontakt lub dodaj nowy:"}
                </h5>
            </div>
            <div className="mt-3">
                <Contacts choosed = {setCustomer} />
            </div>
            <div>
                <LoadingButton disabled = {disabledButton} >
                    Utwórz zamówienie
                </LoadingButton>
            </div>
        </div>
    )
}

export default AddOrder;