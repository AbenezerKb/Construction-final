import api from "./api";

export const register = (data) => api.post("/authorization/register", data);

export const login = (data) => api.post("/authorization/login", data);

export const approve = (data) => api.post("/authorization/approve", data);
