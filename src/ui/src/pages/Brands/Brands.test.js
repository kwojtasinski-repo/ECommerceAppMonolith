import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Brands from "./Brands";
import { BrowserRouter as Router, Route } from "react-router-dom";
import axios from "../../axios-setup";
import { getSampleBrands } from "../../helpers/testFixtures";
import AddBrand from "./Add/AddBrand";

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

describe('Brands component', () => {
    test('should render Brands', async () => {
        axios.get.mockResolvedValue({ data: getSampleBrands() });
        const { container } = render( <Router> <Brands/> </Router> );
        
        await waitFor(() => expect(container.textContent).toContain('Dodaj firmę'));
    });

    test('should show error when url is invalid', async () => {
        axios.get.mockImplementationOnce(() => 
            Promise.reject({
                response: {
                    status: 404,
                    data: {
                        errors: [
                            {
                                code: ''
                            }
                        ]
                    }
                }
            })
        );
        const { container } = render( <Router> <Brands/> </Router> );
        await waitFor(() => expect(container.textContent).toContain('Dodaj firmę'));
        
        const element = await screen.findByText('Sprawdź podany url');
        expect(element).not.toBe(null);
    });
});