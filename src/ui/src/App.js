import './App.css';
import { BrowserRouter as Router, Route, Routes, } from 'react-router-dom';
import Layout from './components/Layout/Layout';
import Header from './components/Header/Header';
import Menu from './components/Menu/Menu';
import Footer from './components/Footer/Footer';
import Searchbar from './components/UI/Searchbar/Searchbar';
import { Suspense, useReducer } from 'react';
import ErrorBoundary from './hoc/ErrorBoundary';
import { initialState, reducer } from './reducer';
import AuthContext from './context/AuthContext';
import NotFound from './pages/404/NotFound';
import Login from './pages/Auth/Login/Login';
import Register from './pages/Auth/Register/Register';
import Item from './pages/Item/Item';
import Home from './pages/Home/Home';

function App() {
  const [state, dispatch] = useReducer(reducer, initialState);
  
  const header = (
    <Header >
      <Searchbar />
    </Header>
  );

  const menu = (
    <Menu />
  )

  const content = (
    <Suspense fallback={<p>Ładowanie...</p>} >
      <Routes>
        <Route path='/items/:id' element = {<Item />} />
        <Route path='/login' element = {<Login />} />
        <Route path='/register' element = {<Register />} />
        <Route path="/" end element = {<Home />} />
        <Route path="*" element = {<NotFound/>} />
      </Routes>
    </Suspense>
  )

  const footer = (
    <Footer />
  )

  return (
    <Router>
      <AuthContext.Provider value = {{
          user: state.user,
          login: (user) => dispatch({ type: "login", user }),
          logout: (user) => dispatch({ type: "logout" })
      }}>
        <ErrorBoundary>
          <Layout 
            header = {header}
            menu = {menu}
            content = {content}
            footer = {footer}
            />
        </ErrorBoundary>
      </AuthContext.Provider>
    </Router>
  );
}

export default App;
