import { render, screen, fireEvent } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import BrandForm from "./BrandForm";

describe('BrandForm component', () => {
    test('renders BrandForm', () => {
        render( <Router> <BrandForm cancelEditUrl = '/' /> </Router>);

        expect(screen.getByLabelText(/nazwa/i)).toBeInTheDocument();
    });

    test('should change name', () => {
        render( <Router> <BrandForm cancelEditUrl = '/' /> </Router>);
        const nameInput = screen.getByLabelText('Nazwa');
        const name = 'example';

        fireEvent.change(nameInput, {target: {value: name} });

        expect(nameInput.value).toBe(name);
    });

    test('should show error when pass invalid name', () => {
        render( <Router> <BrandForm cancelEditUrl = '/' /> </Router>);
        const nameInput = screen.getByLabelText('Nazwa');
        const name = 'a';

        fireEvent.change(nameInput, {target: {value: name} });

        expect(nameInput.className).toContain('invalid');
    });

    test('should render BrandForm with input data', () => {
        const brand = { id: 1, name: 'Brand #1' };
        render( <Router> <BrandForm cancelEditUrl = '/' brand = {brand} /> </Router>);
        const nameInput = screen.getByLabelText('Nazwa');

        expect(nameInput.value).toBe(brand.name);
    });
});