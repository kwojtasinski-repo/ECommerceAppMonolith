import './App.css';
import Layout from './components/Layout/Layout';
import Header from './components/Header/Header';
import Menu from './components/Menu/Menu';
import Footer from './components/Footer/Footer';
import Searchbar from './components/Searchbar/Searchbar';

function App() {
  const header = (
    <Header >
      <Searchbar />
    </Header>
  );

  const menu = (
    <Menu />
  )

  const footer = (
    <Footer />
  )

  return (
    <Layout 
      header = {header}
      menu = {menu}
      footer = {footer}
      />
  );
}

export default App;
