import { round } from "./roundHelper";

export function calculateItem(item, currencyRates) {
    const [sourceRate, targetRate] = getSourceAndTargetRate(item.code, "PLN", currencyRates);
    item.cost = calculateCost(item.cost, sourceRate, targetRate);
}

export function calculateItems(items, currencyRates) {
    for (const key in items) {
        let item = items[key];
        calculateItem(item, currencyRates);
    }
}

function getSourceAndTargetRate(sourceCode, targetCode, currencyRates) {
    const sourceRate = currencyRates.find(cr => cr.code.toUpperCase() === sourceCode.toUpperCase());
    const targetRate = currencyRates.find(cr => cr.code.toUpperCase() === targetCode.toUpperCase());
    return [sourceRate, targetRate];
}

function calculateCost(cost, sourceRate, targetRate) {
    cost = cost * sourceRate.rate / targetRate.rate;
    cost = round(cost, 2);
    return cost;
}