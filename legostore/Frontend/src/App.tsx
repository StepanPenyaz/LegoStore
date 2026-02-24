import { Provider } from 'react-redux';
import { store } from './Store/store';
import { StorageViewer } from './Components/StorageViewer';
import './index.css';

function App() {
  return (
    <Provider store={store}>
      <StorageViewer />
    </Provider>
  );
}

export default App;
