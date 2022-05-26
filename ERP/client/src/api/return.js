import api from "./api";

export const fetchReturns = () => api.get("/return").then((res) => res.data);

export const fetchReturn = (id) => api.get(`/return/${id}`).then((res) => res.data);

export const fetchDamages = () => api.get("/return/damage").then((res) => res.data);

export const fetchBorrowedAssets = (data) =>
    api.post("/return/assets", data).then((res) => res.data);

export const returnReturn = (data) => api.post("/return/return", data);
