import axios from "./axios-setup";

export async function getRecommendationProducts() {
    try {
        const response = await axios.get('/purchase-profiler-module/recomendations');
        return response.data;
    } catch {
        return [];
    }
}
