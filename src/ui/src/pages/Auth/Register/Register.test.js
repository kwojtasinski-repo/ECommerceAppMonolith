import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router";
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
        render( <Router> <Register/> </Router> );
        const emailInput = screen.getByLabelText('Email');
        const email = 'example@gmail.com';

        fireEvent.change(emailInput, {target: {value: email} });

        expect(emailInput.value).toBe(email);
    });

    test('should change password', () => {
        render( <Router> <Register/> </Router> );
        const passwordInput = screen.getByLabelText('Hasło');
        const password = 'PasW0Rd!@241abc';

        fireEvent.change(passwordInput, {target: {value: password} });

        expect(passwordInput.value).toBe(password);
    });

    test('should show error when passed invalid email', () => {
        render( <Router> <Register/> </Router> );
        const emailInput = screen.getByLabelText('Email');
        const email = 'example';

        fireEvent.change(emailInput, {target: {value: email} });

        expect(emailInput.className).toContain('is-invalid');
    });

    test('should show error when passed invalid password', () => {
        render( <Router> <Register/> </Router> );
        const passwordInput = screen.getByLabelText('Hasło');
        const password = 'password';

        fireEvent.change(passwordInput, {target: {value: password} });

        expect(passwordInput.className).toContain('is-invalid');
    });

    test('should send data to Api with valid data', async () => {
        render( <Router> <Register/> </Router> );
        const emailInput = screen.getByLabelText('Email');
        const email = 'example@gmail.com';
        const passwordInput = screen.getByLabelText('Hasło');
        const password = 'PasW0Rd!@241abc';
        axios.post.mockImplementation(() => 
            Promise.resolve()
        );
        const submitButton = screen.getByText('Zarejestruj');

        fireEvent.change(emailInput, {target: {value: email} });
        fireEvent.change(passwordInput, {target: {value: password} });
        
        fireEvent.click(submitButton);
        await waitFor(() => expect(axios.post).toHaveBeenCalledTimes(1));

        const linkElement = screen.getByText(/Ładowanie/i);
        expect(linkElement).toBeInTheDocument();
    });
});