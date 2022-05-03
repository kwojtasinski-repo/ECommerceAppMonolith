import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Login from "./Login";
import { BrowserRouter as Router } from "react-router-dom";
import axios from "../../../axios-setup";

jest.mock('axios', () => {
    return {
        create: () => {
            return {
                post: jest.fn(() => Promise.resolve('')),
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

describe('Login component', () => {
    test('renders Logowanie', () => {
        render( <Router> <Login/> </Router> );

        expect(screen.getByText(/logowanie/i)).toBeInTheDocument();
    });

    test('should change email', () => {
        const utils = render( <Router> <Login/> </Router> );
        const emailInput = utils.getByLabelText('Email');
        const email = 'example@gmail.com';

        fireEvent.change(emailInput, {target: {value: email} });

        expect(emailInput.value).toBe(email);
    })

    test('pass invalid credentials should show error', async () => {
        axios.post.mockImplementation(() => 
            Promise.reject({
                response: {
                    status: 400,
                    data: {
                        errors: [
                            {
                                code: 'invalid_credentials'
                            }
                        ]
                    }
                }
            })
        );
        const utils = render( <Router> <Login/> </Router> );
        const submitButton = utils.getByText('Zaloguj');

        fireEvent.click(submitButton);
        await waitFor(() => expect(axios.post).toHaveBeenCalledTimes(1));

        expect(screen.getByText('Niepoprawne dane logowania, sprawdź czy email i hasło są poprawne')).toBeInTheDocument();
    });
});