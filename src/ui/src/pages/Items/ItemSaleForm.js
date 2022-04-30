import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import Input from "../../components/Input/Input";
import LoadingButton from "../../components/UI/LoadingButton/LoadingButton";
import { mapToMessage, validate } from "../../helpers/validation";

function ItemSaleForm(props) {
    const [currencies, setCurrencies] = useState(props.currencies);
    const [loadingButton, setLoadingButton] = useState(false);
    const [form, setForm] = useState({
        itemId: {
            value: ''
        }, 
        itemCost: {
            value: '',
            error: '',
            showError: false,
            rules: ['required']
        },
        currencyCode: {
            value: '',
            error: '',
            showError: false,
            rules: ['required', { rule: 'only', length: 3 }]
        }
    });
    const [error, setError] = useState('');

    const changeHandler = (value, fieldName) => {
        const error = validate(form[fieldName].rules, value);
        setForm({
            ...form, 
            [fieldName]: {
                ...form[fieldName],
                value,
                showError: true,
                error: error
            } 
        });
    };

    useEffect(() => {
        if (currencies && currencies.length > 0) {
            const currency = currencies.find(c => true);
            setForm({
                ...form,
                currencyCode: {
                    ...form.currencyCode,
                    value: currency.code
                }
            })
        }
    }, [currencies])

    useEffect(() => {
        if (props.itemSale) {
            setForm({
                ...form,
                itemId: {
                    value: props.itemSale.id
                },
                itemCost: {
                    ...form.itemCost,
                    value: props.itemSale.cost
                },
                currencyCode: {
                    ...form.currencyCode,
                    value: props.itemSale.code
                }
            })
        }
    }, [props.itemSale])

    const submit = async (event) => {
        event.preventDefault();
        setLoadingButton(true);

        try {   
            await props.onSubmit({
                itemId: form.itemId.value,
                itemCost: form.itemCost.value,
                currencyCode: form.currencyCode.value
            });
        } catch (exception) {
            console.log(exception);
            let errorMessage = '';
            const status = exception.response.status;
            const errors = exception.response.data.errors;
            
            for(const errMsg in errors) {
                errorMessage += mapToMessage(errors[errMsg].code, status);
            }

            setError(errorMessage);
            setLoadingButton(false);
        }
    }

    return (
        <form onSubmit={submit} >
            {error ? (
                <div className="alert alert-danger">{error}</div>
            ) : null}
            <div>
                <Input label = "Cena"
                        type = "number"
                        value = {form.itemCost.value}
                        error = {form.itemCost.error}
                        showError = {form.itemCost.showError}
                        onChange = {val => changeHandler(val, 'itemCost')} />
                <Input label = "Waluta"
                        type = "select" 
                        value = {form.currencyCode.value}
                        error = {form.currencyCode.error}
                        showError = {form.currencyCode.showError}
                        options = { currencies.map(t => {
                            return {
                                key: t.id,
                                value: t.code,
                                label: t.code
                            }
                        })} 
                        onChange = {val => changeHandler(val, 'currencyCode')} />
            </div>
            <div className="text-end mt-2">
                <LoadingButton
                    loading={loadingButton} 
                    className="btn btn-success">
                    {props.textSubmit}
                </LoadingButton>
                <NavLink className="btn btn-secondary ms-2" to = '/items' >{props.textCancel}</NavLink>
            </div>
        </form>
    )
}

export default ItemSaleForm;

ItemSaleForm.defaultProps  = {
    currencies: []
}