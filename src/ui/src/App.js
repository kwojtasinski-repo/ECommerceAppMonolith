import './App.css';
import { BrowserRouter as Router, Route, Routes, } from 'react-router';
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
import Notification from './components/Notification/Notification';
import NotificationContext from './context/NotificationContext';
import AddItem from './pages/Items/AddItem/AddItem';
import Items from './pages/Items/Items';
import RequireAuth from './hoc/RequireAuth';
import EditItem from './pages/Items/EditItem/EditItem';
import PutItemForSale from './pages/Items/PutItemForSale/PutItemForSale';
import Search from './pages/Search/Search';
import Profile from './pages/Profile/Profile';
import ProfileDetails from './pages/Profile/ProfileDetails/ProfileDetails';
import ContactData from './pages/Profile/ContactData/ContactData';
import Cart from './pages/Cart/Cart';
import Currencies from './pages/Currencies/Currencies';
import EditCurrency from './pages/Currencies/EditCurrency/EditCurrency';
import AddCurrency from './pages/Currencies/AddCurrency/AddCurrency';
import CartSummary from './pages/Cart/Finalize/CartSummary';
import OrderItemArchive from './pages/Archive/OrderItemArchive';
import AddOrder from './pages/Order/AddOrder/AddOrder';
import EditContact from './pages/Profile/ContactData/EditContact/EditContact';
import AddOrderContact from './pages/Order/Contact/AddOrderContact';
import EditOrderContact from './pages/Order/Contact/EditOrderContact';
import AddContact from './pages/Profile/ContactData/AddContact/AddContact';
import Order from './pages/Order/Order';
import EditOrder from './pages/Order/EditOrder/EditOrder';
import AddPayment from './pages/Payments/AddPayment';
import MyOrders from './pages/Order/MyOrders/MyOrders';
import ChangeCurrency from './pages/Payments/ChangeCurrency/ChangeCurrency';
import ItemDetails from './pages/Items/ItemDetails/ItemDetails';
import ItemsForSale from './pages/Items/ItemsForSale/ItemsForSale';
import ItemForSaleEdit from './pages/Items/ItemsForSale/ItemForSaleEdit/ItemForSaleEdit';
import Brands from './pages/Brands/Brands';
import Types from './pages/Types/Types';
import TypeEdit from './pages/Types/Edit/TypeEdit';
import TypeAdd from './pages/Types/Add/TypeAdd';
import EditBrand from './pages/Brands/Edit/EditBrand';
import AddBrand from './pages/Brands/Add/AddBrand';
import Users from './pages/Users/Users';
import EditUser from './pages/Users/EditUser/EditUser';
import RequirePermission from './hoc/RequirePermission';
import initializeApp from './appInitializer';
import RecommendedCarousel from './components/RecommendCarousel/RecommendCarousel';

function App() {
  const [state, dispatch] = useReducer(reducer, initialState);
  const [notifications, setNotifications] = useState([]);
  
  const addNotification = (notification) => {
    const newNotification = [...notifications, notification]
    setNotifications(newNotification);
  };
  const deleteNotification = (id) => {
    setNotifications(notifications.filter(n => n.id !== id));
  };

  const intialize = async () => {
    try {
      const app = await initializeApp()
      dispatch({ type: 'initialized', app });
    } catch {
      dispatch('initialized');
    }
  };

  useEffect(() => {
    if (!state.initializing) {
      return;
    }

    intialize();
  }, [state.initializing]);

  const header = (
    <Header >
      <Searchbar />
    </Header>
  );

  const menu = (
    <Menu />
  )
  const showRecommendationItems = window._env_?.REACT_APP_SHOW_RECOMMEND_ITEMS ? window._env_.REACT_APP_SHOW_RECOMMEND_ITEMS : process.env.REACT_APP_SHOW_RECOMMEND_ITEMS;
  const content = (
    <Suspense fallback={<p>Ładowanie...</p>} >
      {showRecommendationItems ?
        <div className="container"><RecommendedCarousel /></div>
        : null
      }
      <Routes>
        <Route path='/items/:id' element = {<Item />} />
        <Route path='/search' element = {<Search />} >  
          <Route path=":term" element = {<Search />} />
          <Route path="" element = {<Search />} />
        </Route>

        <Route element = {<RequireAuth />}>

          <Route element = {<RequirePermission policies = {['items']} /> }>
            <Route path='/brands' element = {<Brands />} >
              <Route path='edit/:id' element = {<EditBrand />} />
              <Route path='add' element = {<AddBrand />} />
            </Route>

            <Route path='/types' element = {<Types />} >
              <Route path='edit/:id' element = {<TypeEdit />} />
              <Route path='add' element = {<TypeAdd />} />
            </Route>

            <Route path='/items' element = {<Items />} />
            <Route path='/items/edit/:id' element = {<EditItem /> } />
            <Route path='/items/add' element = {<AddItem />}/>
            <Route path='/items/details/:id' element = {<ItemDetails /> } />
          </Route>

          <Route element = {<RequirePermission policies = {['items', 'item-sale']} /> }>
            <Route path='/items/sale/edit/:id' element = {<ItemForSaleEdit />  } />
            <Route path='/items/for-sale/:id' element = {<PutItemForSale /> } />
            <Route path='/items/sale' element = {<ItemsForSale />  } >
              <Route path=':term' element = {<ItemsForSale />  }/>
              <Route path='' element = {<ItemsForSale />  }/>
            </Route>
          </Route>

          <Route element = {<RequirePermission policies = {['currencies']} /> }>
            <Route path='/currencies' element = {<Currencies />} >
              <Route path='edit/:id' element = {<EditCurrency />} />
              <Route path='add' element = {<AddCurrency />} />
            </Route>
          </Route>

          <Route element = {<RequirePermission policies = {['users']} /> }>
            <Route path="/users/edit/:id" element = {<EditUser />}/>
            <Route path="/users" element = {<Users />}/>
          </Route>


          <Route path='/payments/add/:id' element = {<AddPayment />} >
            <Route path='change-currency' element = {<ChangeCurrency/>} />
          </Route>

          <Route path='/orders/add' element = {<AddOrder />} >
            <Route path='add-contact' element = {<AddOrderContact />} />
            <Route path='edit-contact/:id' element = {<EditOrderContact />} />
          </Route>

          <Route path='/orders/edit/:id' element = {<EditOrder />} />
          <Route path='/orders/:id' element = {<Order/>}/>
          <Route path='/orders' element = {<MyOrders/>} />
          <Route path='/archive/items/:id' element = {<OrderItemArchive /> } />

          <Route path="profile" element = {<Profile/>}  >
            <Route path="contact-data" element = {  <ContactData /> } >
              <Route path="add" element = { <AddContact />} />
              <Route path="edit/:id" element = { <EditContact />} />
            </Route>
            <Route path="" element = { <ProfileDetails/>} />
          </Route>
          
          <Route path='/cart/summary' element = {<CartSummary/>} />
        </Route>


        <Route path='/cart' element = { <Cart /> } />
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
          logout: () => dispatch({ type: "logout" }),
          intializing: state.initializing
      }}>
        <ReducerContext.Provider value={{
          state: state,
          dispatch: dispatch
        }} >
          <NotificationContext.Provider value={{
            notifications: notifications,
            addNotification: addNotification,
            deleteNotification: deleteNotification
          }}>
            <ErrorBoundary>
              <Layout 
                header = {header}
                menu = {menu}
                content = {content}
                footer = {footer}
                />

              {notifications.map(({ id, color, text, timeToClose }) => (
                    <Notification key={id} id={id} color={color} text={text} timeToClose={timeToClose} />
              ))}
            </ErrorBoundary>
          </NotificationContext.Provider>
        </ReducerContext.Provider>
      </AuthContext.Provider>
    </Router>
  );
}

export default App;
