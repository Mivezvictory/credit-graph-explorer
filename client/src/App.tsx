//import AppLayout from './components/AppLayout';
import { useRoutesElement } from './app/routes';

export function App() {
  const element = useRoutesElement();
  //if(element == )
  console.log(element.type)
  return element;
}

