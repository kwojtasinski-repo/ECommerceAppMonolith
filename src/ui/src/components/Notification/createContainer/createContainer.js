import styles from './container.module.css';

export default function createContainer() {
    const notifyContainerId = "notifyContainer";
    let element = document.getElementById(notifyContainerId);

    if (element) {
        return element;
    }
    
    element = document.createElement("div");
    element.setAttribute("id", notifyContainerId);
    element.className = styles.position;
    document.body.appendChild(element);

    return element;
}