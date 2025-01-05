import axios from "./axios-setup";
import { requestPath } from "./constants";

export async function getRecommendationProducts() {
    try {
        const response = await axios.get(requestPath.purchaseProfilerModule.recommendations);
        return response.data;
    } catch {
        return [];
    }
}
