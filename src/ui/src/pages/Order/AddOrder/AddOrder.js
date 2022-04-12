import useAuth from "../../../hooks/useAuth";
import OrderForm from "../OrderForm";

function AddOrder(props) {
    const [auth] = useAuth();

    const submit = () => {

    }

    return (
        <div className="card">
            <div className="card-header">
                Nowe zamówienie
            </div>
            <div className="card-body">
                <OrderForm
                    buttonText = "Dodaj"
                    onSubmit = {submit} />
            </div>
        </div>
    )
}

export default AddOrder;