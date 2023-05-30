import { BrowserRouter } from 'react-router-dom';
import './App.css';
import Menu from './component/Menu'
import TestComponent from './component/TestComponent';

function App() {
  return (
    <>
    <BrowserRouter basename="/ghpageTest">
      <Menu/>      
    </BrowserRouter>
      
    </>
  );
}

export default App;
