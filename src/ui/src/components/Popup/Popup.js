import styles from "./Popup.module.css";
import PropTypes from "prop-types";

function Popup(props) {
    
    return (
        <div className={styles.popupBox}>
            <div className={styles.box}>
                {props.content}
                <button className={`btn btn-${props.type}`} onClick = {props.handleConfirm} >Potwierdź</button>
                <button className="btn btn-secondary ms-2" onClick = {props.handleClose} >Anuluj</button>
            </div>
        </div>
    )
}

export default Popup;

export const Type = {
    info: "primary",
    success: "success",
    warning: "warning",
    alert: "danger",
};

Popup.propTypes = {
    type: PropTypes.oneOf(Object.keys(Type))
};

Popup.defaultProps = {
    type: Type.info
}