import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Login from "./Login";
import { BrowserRouter as Router } from "react-router";
import axios from "../../../axios-setup";
import { mapCodeToMessage } from "../../../helpers/errorCodeMapper";

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
        render( <Router> <Login/> </Router> );
        const emailInput = screen.getByLabelText('Email');
        const email = 'example@gmail.com';

        fireEvent.change(emailInput, {target: {value: email} });

        expect(emailInput.value).toBe(email);
    })

    test('pass invalid credentials should show error', async () => {
        const errorCode = 'invalid_credentials';
        axios.post.mockImplementationOnce(() => 
            Promise.reject({
                response: {
                    status: 400,
                    data: {
                        errors: [
                            {
                                code: errorCode
                            }
                        ]
                    }
                }
            })
        );
        render( <Router> <Login/> </Router> );
        const submitButton = screen.getByText('Zaloguj');

        fireEvent.click(submitButton);
        await waitFor(() => expect(axios.post).toHaveBeenCalledTimes(1));

        expect(screen.getByText(mapCodeToMessage(errorCode))).toBeInTheDocument();
    });

    test('should login after fill form with valid data', async () => {
        render( <Router> <Login/> </Router> );
        const emailInput = screen.getByLabelText('Email');
        const email = 'example@gmail.com';
        const passwordInput = screen.getByLabelText('Hasło');
        const password = 'PasW0Rd!@241abc';
        axios.post.mockImplementationOnce(() => 
            Promise.resolve({
                data: {
                    email,
                    accessToken: 'token',
                    id: 1,
                    claims: [],
                    expires: 1
                }
            }
        ));
        const submitButton = screen.getByText('Zaloguj');
        fireEvent.change(emailInput, {target: {value: email} });
        fireEvent.change(passwordInput, {target: {value: password} });

        fireEvent.click(submitButton);
        await waitFor(() => expect(axios.post).toHaveBeenCalledTimes(1));

        const linkElement = screen.getByText(/Ładowanie/i);
        expect(linkElement).toBeInTheDocument();
    });
});