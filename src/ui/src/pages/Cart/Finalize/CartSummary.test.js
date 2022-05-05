import { render, waitFor } from "@testing-library/react";
import CartSummary from "./CartSummary";
import { BrowserRouter as Router } from "react-router-dom";
import axios from "../../../axios-setup";
import { getSampleOrderItems } from "../../../helpers/testFixtures";

jest.mock('axios', () => {
    return {
        create: () => {
            return {
                get: jest.fn(() => Promise.resolve('')),
                interceptors: {
                    request: { use: jest.fn(), eject: jest.fn() },
                    response: { use: jest.fn(), eject: jest.fn() }
                }
            }
        },
        interceptors: {
            request: { use: jest.fn(), eject: jest.fn() },
            response: { use: jest.fn(), eject: jest.fn() }
        }
    }
});

describe('CartSummary component', () => {
    test('should render CartSummary', async () => {
        axios.get.mockResolvedValue({ data: [] });
        const { container } = render( <Router> <CartSummary/> </Router> );
        
        await waitFor(() => expect(container.textContent).toContain('Aktualnie nie masz żadnych przedmiotów w trakcie realizacji zamówienia'));
    });

    test('should render CartSummary with data from Api', async () => {
        axios.get.mockResolvedValue({ data: getSampleOrderItems() });
        const { container } = render( <Router> <CartSummary/> </Router> );

        await waitFor(() => expect(container.textContent).toContain('Podsumowanie'));
    });
});