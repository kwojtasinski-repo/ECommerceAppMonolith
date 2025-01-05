export const reducer = (state, action) => {
    switch (action.type) {
        case 'login' :
            return { ...state, user: action.user }
        case 'logout' : 
            return { ...state, user: null }
        case 'modifiedState' :
            let events = state.events;
            
            if (events.length === 5) {
                events = [];
            }

            events.push(action.currentEvent);
            return { ...state, events: events }
        case 'initialized' :
            const user = action?.app?.user ?? null;
            const recommendationProducts = action?.app?.recommendationProducts ?? [];
            return { ...state, user, recommendationProducts, initializing: false }
        default :
            throw new Error(`Action ${action.type} doesnt exists.`);
    }
}

export const initialState = {
    user: null,
    currentEvent: '',
    events: [],
    initializing: true,
    recommendationProducts: []
};
