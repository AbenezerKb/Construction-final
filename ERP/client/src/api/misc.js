import api from "./api";

export const fetchCompanyNamePrefix = () => api.get("/misc/company").then((res) => res.data);

export const updateCompanyNamePrefix = (data) => api.post("/misc/company", data);
