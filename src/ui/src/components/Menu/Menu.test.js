import { render, screen } from '@testing-library/react';
import Menu from './Menu';
import { BrowserRouter as Router } from 'react-router-dom';
import AuthContext from '../../context/AuthContext';

describe('Menu component', () => {
    test("renders Zaloguj if user is null", () => {
        render(<Router> <Menu /> </Router>);
        const link = screen.getByText(/zaloguj/i);
        expect(link).toBeInTheDocument();
    });

    test("renders Wyloguj if user exists", () => {
        render(<AuthContext.Provider value = {{
            user: { email: 'email@email.com'},
            login: () => {},
            logout: () => {}
        }}>
            <Router> <Menu /> </Router>
        </AuthContext.Provider>);
        const link = screen.getByText(/wyloguj/i);
        expect(link).toBeInTheDocument();
    });

   test("renders Przedmioty if user has permissions", () => {
        render(<AuthContext.Provider value = {{
            user: { email: 'email@email.com', claims: { permissions: [ 'items'] }},
            login: () => {},
            logout: () => {}
        }}>
            <Router> <Menu /> </Router>
        </AuthContext.Provider>);
        const link = screen.getByText(/przedmioty/i); 
   }) 

   test("renders Waluty if user has permissions", () => {
        render(<AuthContext.Provider value = {{
            user: { email: 'email@email.com', claims: { permissions: [ 'currencies'] }},
            login: () => {},
            logout: () => {}
        }}>
            <Router> <Menu /> </Router>
        </AuthContext.Provider>);
        const link = screen.getByText(/waluty/i);
   }) 
});