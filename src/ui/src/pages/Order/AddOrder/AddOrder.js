import axios from "../../../axios-setup";
import { useState } from "react";
import LoadingButton from "../../../components/UI/LoadingButton/LoadingButton";
import Contacts from "../Contact/Contacts";
import { useNavigate } from "react-router";
import { mapToMessage } from "../../../helpers/validation";
import { requestPath } from "../../../constants";

function AddOrder() {
    const [customer, setCustomer] = useState(null);
    const disabledButton = customer === null;
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const submit = async () => {
        setLoading(true);
        try {
            const response = await axios.post(requestPath.salesModule.orders, {
                customerId: customer.id, 
                currencyCode: "PLN"
            });        
            const id = response.headers.location.split('/sales-module/orders/')[1];
            navigate(`/orders/${id}`);
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status) + '\n';
            }
            setError(errorMessage);
            setLoading(false);
        }
    }
    
    return (
        <div>
            {error ? (
                <div className="alert alert-danger">
                    {error}
                </div>
            ) : null}
            <div>
                <h5>
                    {customer !== null ? `Wybrano: ${customer.firstName} ${customer.lastName} ${customer.companyName ? customer.companyName : ""}` : "Wybierz kontakt lub dodaj nowy:"}
                </h5>
            </div>
            <div className="mt-3">
                <Contacts choosed = {setCustomer} />
            </div>
            <div>
                <LoadingButton loading = {loading}
                               disabled = {disabledButton} onClick = {submit} >
                    Utwórz zamówienie
                </LoadingButton>
            </div>
        </div>
    )
}

export default AddOrder;