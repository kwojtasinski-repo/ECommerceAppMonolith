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