import { render, screen } from '@testing-library/react';
import App from './App';

test('should get text in the process of loading page', () => {
  render(<App />);
  const linkElement = screen.getByText(/Ładowanie/i);
  expect(linkElement).toBeInTheDocument();
});
