import api from "./api";

export const fetchStores = () => api.get("/store").then((res) => res.data);
