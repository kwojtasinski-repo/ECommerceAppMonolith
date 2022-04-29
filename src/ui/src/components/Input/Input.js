const InputTextArea = props => {
    return (
        <div className="form-group">
            <label>{props.label}</label>
            <textarea
                value={props.value}
                onChange={event => props.onChange(event.target.value)}
                type={props.type}
                className={`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}  />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputText = props => {
    return (
        <div className="form-group">
            <label>{props.label}</label>
            <input 
                type = {props.type}
                value = {props.value}
                className = {`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                onChange = { event => props.onChange(event.target.value) } />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputZipCode = props => {
    const keyPress = event => {
        const regex = new RegExp(pattern);
        const key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        const newValue = event.target.value + key;
        
        if (!regex.test(newValue)) {
            event.preventDefault ? event.preventDefault() : event.returnValue = false;
        }
    }

    const keyUp = event => {
        var key = event.keyCode;

        if (key === 8 || key === 46) {
            return;
        }

        let value = event.target.value;
        
        if (value.length === 2) {
            event.target.value += '-';
        }
    }

    const pattern="^\\d{1,2}(?:\\-\\d{0,3})?$";

    return (
        <div className="form-group">
            <label>{props.label}</label>
            <input 
                type = {props.type}
                value = {props.value}
                className = {`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                onKeyPress = { keyPress }
                onKeyUp = { keyUp }
                onChange = { event => props.onChange(event.target.value) } 
                pattern = { props.pattern } />
            <div className="invalid-feedback">
                {props.error}
            </div>
        </div>
    );
}

const InputSelect = props => {
    return (
        <div className="form-group">
            <label>{props.label}</label>
            <select 
                value={props.value} 
                onChange={event => props.onChange(event.target.value)}
                className={`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}>
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
                        id={option.value} />
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

    return (
        <div className="form-group">
            <label>{props.label}</label>
            <input type = "number"
                   value = {props.value}
                   className = {`form-control ${props.error && props.showError ? 'is-invalid' : ''}`}
                   onChange = { event => props.onChange(event.target.value) } 
                   onInput = {event => onInput(event.target.value)} />
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