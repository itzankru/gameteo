import "./app.css";
import { Provider } from "react-redux";
import store from "./store";
import RatePage from "../rates/rates-browse/rates-browse";
import Stub from "../stub/stub";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Header from "../header/header";
import RateLine from "../rates/rate-line/rate-line";
const router = createBrowserRouter([
  {
    path: "/",
    element: <RatePage></RatePage>,
    errorElement: <h3>ERROR PAGE</h3>,
  },
  {
    path: "/rate",
    element: <div></div>,
  },
  {
    path: "/archive",
    element: <h3>ARHIVE</h3>,
  },
  {
    path: "/contacts",
    element: <Stub title="sfsdf"></Stub>,
  },
]);

function App() {
  return (
    <div className="app__container">
      <Provider store={store}>
        <Header></Header>
        <RateLine></RateLine>
        <RouterProvider router={router} />
      </Provider>
    </div>
  );
}

export default App;
