import React from "react";

const NotificationContext = React.createContext({
    notifications: [],
    addNotification: () => {},
    deleteNotification: () => {}
});

NotificationContext.displayName = 'NotificationContext';

export default NotificationContext;