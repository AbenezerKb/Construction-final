import React from "react";
import { AuthProvider } from "./AuthContext";

export default function ContextProviders({ children }) {
    return (
            <AuthProvider>{children}</AuthProvider>
    );
}
