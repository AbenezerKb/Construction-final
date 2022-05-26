import api from "./api";

export const fetchDamagedAssets = (data) =>
    api.get(`/maintenance/assets?modelId=${data}`).then((res) => res.data);

export const fetchMaintenances = () => api.get("/maintenance").then((res) => res.data);

export const fetchMaintenance = (id) => api.get(`/maintenance/${id}`).then((res) => res.data);

export const requestMaintenance = (data) => api.post("/maintenance/request", data);

export const approveMaintenance = (data) => api.post("/maintenance/approve", data);

export const declineMaintenance = (data) => api.post("/maintenance/decline", data);

export const fixMaintenance = (data) => api.post("/maintenance/fix", data);
