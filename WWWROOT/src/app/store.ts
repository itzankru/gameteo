import { configureStore } from "@reduxjs/toolkit";
import { createLogger } from "redux-logger";
import rateApi from "../rates/rates-api";

const logger = createLogger({
  // ...options
});

export default configureStore({
  reducer: {
    [rateApi.reducerPath]: rateApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(rateApi.middleware, logger),
});
