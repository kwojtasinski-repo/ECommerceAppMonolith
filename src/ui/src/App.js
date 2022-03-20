import './App.css';
import Layout from './components/Layout/Layout';
import Header from './components/Header/Header';
import Menu from './components/Menu/Menu';
import Footer from './components/Footer/Footer';
import Searchbar from './components/UI/Searchbar/Searchbar';
import Items from './components/Items/Items';
import axios from './axios-setup';
import { useEffect, useState } from 'react';
import { mapToItems } from './helpers/mapper';
import ErrorBoundary from './hoc/ErrorBoundary';

function App() {
  const [items, setItems] = useState([]);

  const fetchItems = async () => {
    const response = await axios.get(`/items-module/item-sales`);
    const items = mapToItems(response.data);
    setItems(items);
  };

  
  useEffect(() => {
    fetchItems();
  }, []);

  const header = (
    <Header >
      <Searchbar />
    </Header>
  );

  const menu = (
    <Menu />
  )

  const content = (
    <Items items={items} />
  )

  const footer = (
    <Footer />
  )

  return (
    <ErrorBoundary>
      <Layout 
        header = {header}
        menu = {menu}
        content = {content}
        footer = {footer}
        />
    </ErrorBoundary>
  );
}

export default App;
