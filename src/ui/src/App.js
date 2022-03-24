import './App.css';
import { BrowserRouter as Router, Route, Routes, } from 'react-router-dom';
import Layout from './components/Layout/Layout';
import Header from './components/Header/Header';
import Menu from './components/Menu/Menu';
import Footer from './components/Footer/Footer';
import Searchbar from './components/UI/Searchbar/Searchbar';
import { Suspense, useEffect, useReducer, useState } from 'react';
import ErrorBoundary from './hoc/ErrorBoundary';
import { initialState, reducer } from './reducer';
import AuthContext from './context/AuthContext';
import NotFound from './pages/404/NotFound';
import Login from './pages/Auth/Login/Login';
import Register from './pages/Auth/Register/Register';
import Item from './pages/Item/Item';
import Home from './pages/Home/Home';
import ReducerContext from './context/ReducerContext';
import Notification, { Color } from './components/Notification/Notification';

function App() {
  const [state, dispatch] = useReducer(reducer, initialState);
  const [notifications, setNotifications] = useState([]);
  
  const header = (
    <Header >
      <Searchbar />
    </Header>
  );

  const menu = (
    <Menu />
  )

  useEffect(() => {
    if (state.user && state.event === "login") {
      setNotifications([...notifications, { color: Color.success, id: notifications.length, text: 'Pomyślnie zalogowano', timeToClose: 5000 } ]);
      state.event = '';
    } 

    if (state.user == null && state.event === "logout") {
      setNotifications([...notifications, { color: Color.info, id: notifications.length, text: 'Wylogowano', timeToClose: 5000 } ]);
      state.event = '';
    }
  }, [state.user]);

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
          logout: () => dispatch({ type: "logout" })
      }}>
        <ReducerContext.Provider value={{
          state: state,
          dispatch: dispatch
        }} >
          <ErrorBoundary>
            <Layout 
              header = {header}
              menu = {menu}
              content = {content}
              footer = {footer}
              />
              
              {notifications.map(({ id, color, text, timeToClose }) => (
                <Notification key={id} color={color} text={text} timeToClose={timeToClose} >
                </Notification>
              ))}
          </ErrorBoundary>
        </ReducerContext.Provider>
      </AuthContext.Provider>
    </Router>
  );
}

export default App;
