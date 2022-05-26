import api from "./api";

export const fetchEmployees = () => api.get("/employee").then((res) => res.data);

export const fetchEmployee = (id) => api.get(`/employee/${id}`).then((res) => res.data);

export const updateEmployee = (data) => api.post("/employee/update", data);
