import api from "./api";

export const fetchDamages = () => api.get("/damage").then((res) => res.data);

export const addDamage = (data) => api.post("/damage/add", data);

export const updateDamages = (data) => api.post("/damage/update", data);
