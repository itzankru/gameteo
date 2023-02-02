import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { TDayRates } from "../rates/models/rates-model"; 

// Define a service using a base URL and expected endpoints
export const rateApi = createApi({
  reducerPath: "rateApi",
  baseQuery: fetchBaseQuery({ baseUrl: "http://127.0.1.1:8080/api/currencyrate" }),
  tagTypes: ["Rate"],
  endpoints: (builder) => ({
    getRates: builder.query<TDayRates,string>(
      {
        //2022-01-28
        query: (day:string) => ({
          url: `getratesbyday/`,
          method: "GET",
          params: {
            day: day}
      }),
      providesTags: ["Rate"]
      
    }),
    addRate: builder.mutation({
      //   query: (newRate) => ({
      //     url: "rate",
      //     method: "POST",
      //     body: newRate,
      //   }),
      queryFn: () => ({ data: null }),
      invalidatesTags: ["Rate"],
    }),
  }),
});

// Export hooks for usage in functional components, which are
// auto-generated based on the defined endpoints
export const { useLazyGetRatesQuery, useAddRateMutation } = rateApi;
export default rateApi;
