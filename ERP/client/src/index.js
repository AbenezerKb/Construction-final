import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";

import ContextProviders from "./contexts/ContextProviders";

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: 2,
        },
    },
});

ReactDOM.render(
    <React.StrictMode>
        <ContextProviders>
            <QueryClientProvider client={queryClient}>
                <App />
                <ReactQueryDevtools />
            </QueryClientProvider>
        </ContextProviders>
    </React.StrictMode>,
    document.getElementById("root")
);
