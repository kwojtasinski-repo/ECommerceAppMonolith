import axios from 'axios';

function getToken() {
    const auth = JSON.parse(window.localStorage.getItem('token-data'));
    return auth ? auth.token : null;
}

const token = getToken();

const instance = axios.create({
    baseURL: `${process.env.REACT_APP_BACKEND_URL}`
});

instance.interceptors.request.use((req) => {
    if (token) {
        req.headers.Authorization = `Bearer ${token}`;
    }

    return req;
})

export default instance;