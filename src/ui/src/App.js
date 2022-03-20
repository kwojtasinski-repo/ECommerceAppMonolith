import './App.css';
import Layout from './components/Layout/Layout';
import Header from './components/Header/Header';
import Menu from './components/Menu/Menu';
import Footer from './components/Footer/Footer';
import Searchbar from './components/Searchbar/Searchbar';
import Items from './components/Items/Items';
import axios from './axios-setup';
import { useEffect, useState } from 'react';

function App() {
  const [items, setItems] = useState([]);

  const fetchItems = async () => {
    const response = await axios.get(`/items-module/item-sales`);
    setItems(response.data);
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
    <Layout 
      header = {header}
      menu = {menu}
      content = {content}
      footer = {footer}
      />
  );
}

export default App;
