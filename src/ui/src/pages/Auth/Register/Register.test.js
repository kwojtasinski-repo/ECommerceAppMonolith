import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import Register from "./Register";
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

describe('Register component', () => {
    test('renders Rejestracja', () => {
        render( <Router> <Register/> </Router> );

        expect(screen.getByText(/rejestracja/i)).toBeInTheDocument();
    });

    test('should change Email', () => {
        const utils = render( <Router> <Register/> </Router> );
        const emailInput = utils.getByLabelText('Email');
        const email = 'example@gmail.com';

        fireEvent.change(emailInput, {target: {value: email} });

        expect(emailInput.value).toBe(email);
    });

    test('should change password', () => {
        const utils = render( <Router> <Register/> </Router> );
        const passwordInput = utils.getByLabelText('Hasło');
        const password = 'PasW0Rd!@241abc';

        fireEvent.change(passwordInput, {target: {value: password} });

        expect(passwordInput.value).toBe(password);
    });

    test('should show error when passed invalid email', () => {
        const utils = render( <Router> <Register/> </Router> );
        const emailInput = utils.getByLabelText('Email');
        const email = 'example';

        fireEvent.change(emailInput, {target: {value: email} });

        expect(emailInput.className).toContain('is-invalid');
    });

    test('should show error when passed invalid password', () => {
        const utils = render( <Router> <Register/> </Router> );
        const passwordInput = utils.getByLabelText('Hasło');
        const password = 'password';

        fireEvent.change(passwordInput, {target: {value: password} });

        expect(passwordInput.className).toContain('is-invalid');
    });
});