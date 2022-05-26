import api from "./api"

export const fetchAllPurchases = () =>
  api.get("/purchase").then((res) => res.data)

export const fetchPurchases = (data) =>
  api.post("/purchase", data).then((res) => res.data)

export const fetchPurchase = (id) =>
  api.get(`/purchase/${id}`).then((res) => res.data)

export const requestMaterialPurchase = (data) =>
  api.post("/purchase/request/material", data)

export const requestEquipmentPurchase = (data) =>
  api.post("/purchase/request/equipment", data)

export const requestQueuedPurchase = (data) =>
  api.post("/purchase/requestqueued", data)

export const approvePurchase = (data) => api.post("/purchase/approve", data)

export const declinePurchase = (data) => api.post("/purchase/decline", data)

export const checkPurchase = (data) => api.post("/purchase/check", data)

export const queuePurchase = (data) => api.post("/purchase/queue", data)

export const confirmPurchase = (data) => api.post("/purchase/confirm", data)
