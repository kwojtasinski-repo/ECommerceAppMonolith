import { render, screen } from '@testing-library/react';
import App from './App';
import { useReducer } from 'react';
import { initialState } from './reducer';

jest.mock('axios', () => {
  const mockAxios = {
    create: jest.fn(() => mockAxios),
    get: jest.fn(),
    post: jest.fn(),
    put: jest.fn(),
    delete: jest.fn(),
    interceptors: {
      request: {
        use: jest.fn()
      },
      response: {
        use: jest.fn()
      },
    }
  };
  return mockAxios;
});

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useReducer: jest.fn(),
}));

test('should get text in the process of loading page', () => {
  useReducer.mockReturnValue([initialState, jest.fn()]);
  render(<App />);
  const linkElement = screen.getByText(/Ładowanie/i);
  expect(linkElement).toBeInTheDocument();
});
