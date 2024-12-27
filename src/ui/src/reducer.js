export const reducer = (state, action) => {
    debugger
    switch(action.type) {
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
        default :
            throw new Error(`Action ${action.type} doesnt exists.`);
    }
}

export const initialState = {
    user: null,
    currentEvent: '',
    events: []
};
