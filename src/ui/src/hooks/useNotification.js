import { useContext } from "react";
import NotificationContext from "../context/NotificationContext";

export default function useNotification(props) {
    const notificationContext = useContext(NotificationContext);

    let notifications = notificationContext.notifications;

    const addNotification = (notification) => {
        notificationContext.addNotification(notification);
    }

    const deleteNotification = (id) => {
        notificationContext.deleteNotification(id);
    }

    return {
        notifications,
        addNotification,
        deleteNotification
    };
};
