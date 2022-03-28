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

function Input(props) {
    switch(props.type) {
        case 'password':
            return <InputText {...props} type="password" />;
        case 'email':
            return <InputText {...props} type="email" />;
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