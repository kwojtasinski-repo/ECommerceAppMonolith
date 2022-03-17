import './App.css';
import Layout from './components/Layout/Layout';
import Header from './components/Header/Header';
import Menu from './components/Menu/Menu';
import Footer from './components/Footer/Footer';
import Searchbar from './components/Searchbar/Searchbar';
import Items from './components/Items/Items';

function App() {
  const header = (
    <Header >
      <Searchbar />
    </Header>
  );

  const menu = (
    <Menu />
  )

  const content = (
    <Items />
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
