import axios from "./axios-setup";
import { tokenData } from "./constants";
import { getRecommendationProducts } from "./recommendation-products";

const getUser = async () => {
    const tokenDataFromStorage = window.localStorage.getItem(tokenData);
    if (!tokenDataFromStorage) {
        return null;
    }

    const parsedToken = parseTokenData(tokenDataFromStorage);
    if (!parsedToken) {
        return null;
    }

    const token = parsedToken.token;
    if (!token) {
        return null;
    }

    const jwt = parseJwt(token);
    const response = await axios.get('/users-module/account');
    return {
        ...response,
        data: {
        ...response.data,
        accessToken: token,
        tokenExpiresDate: (jwt.exp ?? 0) * 1000
        }
    };
}
  
const parseTokenData = (tokenData) => {
    try {
      return JSON.parse(tokenData);
    } catch {
      return null
    }
}
  
function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
  
    return JSON.parse(jsonPayload);
}

const initializeApp = async () => {
    try {
        const response = await getUser();
        if (!response || !response.data) {
          return {
            user: null
          };
        }

        const recommendationProducts = await getRecommendationProducts();
        return {
            user: {
                email: response.data.email,
                userId: response.data.id,
                claims: response.data.claims,
                token: response.data.accessToken,
                tokenExpiresDate: response.data.tokenExpiresDate
            },
            recommendationProducts
        }
    } catch(err) {
        console.error(err);
        return {
            user: null
        };
    }
};

export default initializeApp;
