import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { Account, Transaction, CreateTransactionParams } from "./types";

export const backendApi = createApi({
    reducerPath: 'backendApi',
    baseQuery: fetchBaseQuery({ baseUrl: '/' }),
    endpoints: (builder) => ({
        getAccount: builder.query<Account, string>({
            query: (accountId) => `/accounts/${accountId}`,
        }),
        listTransactions: builder.query<Transaction[], void>({
            query: () => '/transactions',
        }),
        createTransaction: builder.mutation<Transaction, CreateTransactionParams>({
            query: (payload) => ({
                url: '/transactions',
                method: 'POST',
                body: payload,
            }),
        }),
    }),
})

export const {
    useGetAccountQuery,
    useListTransactionsQuery,
    useLazyListTransactionsQuery,
    useCreateTransactionMutation,
} = backendApi;