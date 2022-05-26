import api from "./api"

export const fetchAllBuys = () => api.get("/buy").then((res) => res.data)

export const fetchBuys = () => api.post("/buy").then((res) => res.data)

export const fetchBuy = (id) => api.get(`/buy/${id}`).then((res) => res.data)

export const requestBuy = (data) => api.post("/buy/request", data)

export const approveBuy = (data) => api.post("/buy/approve", data)

export const declineBuy = (data) => api.post("/buy/decline", data)

export const checkBuy = (data) => api.post("/buy/check", data)

export const confirmBuy = (data) => api.post("/buy/confirm", data)

export const queueBuy = (data) => api.post("/buy/queue", data)
