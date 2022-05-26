import api from "./api";

export const fetchAllBorrows = () => api.get("/borrow").then((res) => res.data);

export const fetchBorrows = (data) => api.post("/borrow", data).then((res) => res.data);

export const fetchBorrow = (id) => api.get(`/borrow/${id}`).then((res) => res.data);

export const requestBorrow = (data) => api.post("/borrow/request", data);

export const approveBorrow = (data) => api.post("/borrow/approve", data);

export const declineBorrow = (data) => api.post("/borrow/decline", data);

export const handBorrow = (data) => api.post("/borrow/hand", data);
