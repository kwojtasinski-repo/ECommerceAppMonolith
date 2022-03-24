export const reducer = (state, action) => {
    switch(action.type) {
        case 'login' :
            return { ...state, user: action.user, event: 'login' }
        case 'logout' : 
            return { ...state, user: null, event: 'logout' }
        default :
            throw new Error(`Action ${action.type} doesnt exists.`);
    }
}

export const initialState = {
    user: null,
    event: ''
};