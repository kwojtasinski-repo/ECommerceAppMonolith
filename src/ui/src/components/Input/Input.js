import { useState } from "react";
import { createGuid } from "../../helpers/createGuid";
import { isEmpty } from "../../helpers/stringExtensions";

const InputTextArea = props => {
    const htmlFor = replacePolishCharacters(props.label) + '-input';
    return (
        <div className="form-group">
            <label htmlFor={htmlFor}>{props.label}</label>
            <textarea
                id={htmlFor}
                value={props.value}
                onChange={event => props.onChange(event.target.value)}
                type={props.type}
                className={`form-control ${props.error && props.showError ? 'is-invalid' : ''}`} 
                autoComplete={props.autoComplete} />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputText = props => {
    const htmlFor = replacePolishCharacters(props.label) + '-input';
    return (
        <div className="form-group">
            <label htmlFor={htmlFor}>{props.label}</label>
            <input 
                id={htmlFor}
                type = {props.type}
                value = {props.value}
                className = {`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                onChange = { event => props.onChange(event.target.value) }
                autoComplete={props.autoComplete} />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputZipCode = props => {
    const [value, setValue] = useState(props.value || '');
    const htmlFor = replacePolishCharacters(props.label) + '-input';
    const pattern="^\\d{1,2}(?:\\-\\d{0,3})?$";
    const regex = new RegExp(pattern);

    const handleSetNextValue = (nextValue) => {  
        setValue(nextValue);
        props.onChange(nextValue);
    };

    const keyDown = event => {
        if (event.key === 'Backspace' || event.key === 'Delete') {
            return;
        }

        if (event.key === '-') {
            event.preventDefault();
            return;
        }

        const key = event.key;
        let nextValue = event.target.value;
        if (nextValue.length === 2) {
            event.target.value += '-';
            nextValue += '-';
        }
        nextValue += key;
        
        if (!regex.test(nextValue)) {
            event.preventDefault();
            return;
        }
    }

    const handleChange = event => {
        let nextValue = event.target.value;
        handleSetNextValue(nextValue);
    };

    return (
        <div className="form-group">
            <label htmlFor={htmlFor}>{props.label}</label>
            <input 
                id={htmlFor}
                type = {props.type}
                value={value}
                className = {`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                onKeyDown= { keyDown }
                onChange = { event => handleChange(event) } 
                pattern = { props.pattern }
                autoComplete={props.autoComplete} />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputSelect = props => {
    const htmlFor = createGuid('N');
    return (
        <div className="form-group">
            <label htmlFor={htmlFor}>{props.label}</label>
            <select 
                id={htmlFor}
                value={props.value} 
                onChange={event => props.onChange(event.target.value)}
                className={`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                autoComplete={props.autoComplete}>
                    {props.options.map(option => 
                    <option value={option.value} key={option.value}>{option.label}</option>
                    )}
            </select>
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputCheckBox = props => {

    const changeFeatureHandler = event => {
        const value = event.target.value;
        const isChecked = event.target.checked;

        if (isChecked) {
            const newValue = [...props.value, value];
            props.onChange(newValue);
        } else {
            const newValue = props.value.filter(p => p !== value);
            props.onChange(newValue);
        }
    }

    return (
        <div className="form-group" >
            {props.options.map(option => (
                <div className="custom-control custom-checkbox" key={option.value} >
                    <input 
                        type="checkbox"
                        className="custom-control-input"
                        value={option.value}
                        checked={props.value.find(o => o === option.value)}
                        onChange={changeFeatureHandler}
                        id={option.value}
                        autoComplete={props.autoComplete} />
                    <label className="custom-control-label" htmlFor={option.value} >{option.label}</label>
                </div>
            ))}
        </div>
    );
}

const InputInteger = props => {
    const onInput = (value) => {
        return Math.round(value);
    }

    const htmlFor = replacePolishCharacters(props.label) + '-input';

    return (
        <div className="form-group">
            <label htmlFor={htmlFor}>{props.label}</label>
            <input id={htmlFor}
                   type = "number"
                   value = {props.value}
                   className = {`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                   onChange = { event => props.onChange(event.target.value) } 
                   onInput = {event => onInput(event.target.value)}
                   autoComplete={props.autoComplete} />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

function Input(props) {
    switch(props.type) {
        case 'select':
            return <InputSelect {...props} />;
        case 'password':
            return <InputText {...props} type="password" />;
        case 'email':
            return <InputText {...props} type="email" />;
        case 'checkbox':
            return <InputCheckBox {...props} />;
        case 'textarea':
            return <InputTextArea {...props} />;
        case 'zipCode' :
            return <InputZipCode {...props} />;
        case 'integerNumber' :
            return <InputInteger {...props} />;
        default:
            return <InputText {...props} />;
    }
}

Input.defaultProps = {
    type: 'text',
    isValid: true,
    showError: false
}

export default Input;

function replacePolishCharacters(text) {
    if (isEmpty(text)) {
        return text;
    }

    let textModified = text.replace('ą', 'a');
    textModified = textModified.replace('ć', 'c');
    textModified = textModified.replace('ę', 'e');
    textModified = textModified.replace('ł', 'l');
    textModified = textModified.replace('ń', 'n');
    textModified = textModified.replace('ó', 'o');
    textModified = textModified.replace('ś', 's');
    textModified = textModified.replace('ż', 'z');
    textModified = textModified.replace('ź', 'z');
    textModified = textModified.replace('Ą', 'A');
    textModified = textModified.replace('Ć', 'C');
    textModified = textModified.replace('Ę', 'E');
    textModified = textModified.replace('Ł', 'L');
    textModified = textModified.replace('Ń', 'N');
    textModified = textModified.replace('Ó', 'O');
    textModified = textModified.replace('Ś', 'S');
    textModified = textModified.replace('Ż', 'Z');
    textModified = textModified.replace('Ź', 'Z');
    
    return textModified;
} 