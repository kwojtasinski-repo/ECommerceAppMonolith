import { useCallback, useEffect } from "react";

export default function useWebsiteTitle(title) {
    const setTitle = useCallback((newTitle) => {
        document.title = newTitle;
    }, []);

    useEffect(() => {
        if (title) {
            setTitle(title);
        }
    }, [title, setTitle]);

    return setTitle;
}
