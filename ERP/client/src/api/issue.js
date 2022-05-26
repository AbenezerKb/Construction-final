import api from "./api";

export const fetchAllIssues = () => api.get("/issue").then((res) => res.data);

export const fetchIssues = (data) => api.post("/issue", data).then((res) => res.data);

export const fetchIssue = (id) => api.get(`/issue/${id}`).then((res) => res.data);

export const requestIssue = (data) => api.post("/issue/request", data);

export const approveIssue = (data) => api.post("/issue/approve", data);

export const declineIssue = (data) => api.post("/issue/decline", data);

export const handIssue = (data) => api.post("/issue/hand", data);
