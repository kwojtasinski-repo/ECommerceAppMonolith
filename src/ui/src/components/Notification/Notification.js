import PropTypes from "prop-types";
import styles from "./Notification.module.css";
import { ReactComponent as Times } from "./times.svg";
import cn from "classnames";
import { useCallback, useEffect, useState } from "react";
import useNotification from "../../hooks/useNotification";
import createContainer from "./createContainer/createContainer";
import { createPortal } from "react-dom";

const timeToClose = 3000;
const container = createContainer();

function Notification(props) {
    const deleteNotification = useNotification().deleteNotification;
    const [id, setId] = useState(props.id);

    const onDelete = (event) => {
        event.preventDefault();
        deleteNotification(id);
    };
    
    const autoCloseNotification = useCallback((timeToClose) => {
        if (!timeToClose) {
            return undefined;
        }

        return setTimeout(() => {
            deleteNotification(id);
        }, timeToClose);
    }, [id, deleteNotification])

    useEffect(() => setId(props.id), [props.id])

    useEffect(() => {
        if (!props.timeToClose) {
            return;
        }

        const timeoutId = autoCloseNotification(props.timeToClose);
        return () => {
            clearTimeout(timeoutId);
        }
    }, [autoCloseNotification, props.timeToClose]);

    return createPortal(
        <div className={cn([styles.notification, styles[props.color]])}>
            {props.text}
            <button onClick={onDelete} className={styles.closeButton}>
                <Times height={16} />
            </button>
        </div>,
        container
    );
};

export const Color = {
    info: "info",
    success: "success",
    warning: "warning",
    error: "error",
};

Notification.propTypes = {
    notificationType: PropTypes.oneOf(Object.keys(Color)),
    text: PropTypes.string,
    timeToClose: PropTypes.number
};

Notification.defaultProps = {
    color: Color.info,
    text: 'This is notification',
    timeToClose: timeToClose
}

export default Notification;