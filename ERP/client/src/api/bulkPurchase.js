import api from "./api"

export const fetchBulkPurchases = () =>
  api.get("/bulkPurchase").then((res) => res.data)

export const fetchBulkPurchase = (id) =>
  api.get(`/bulkPurchase/${id}`).then((res) => res.data)

export const requestBulkPurchase = (data) =>
  api.post("/bulkPurchase/request", data)

export const approveBulkPurchase = (data) =>
  api.post("/bulkPurchase/approve", data)

export const declineBulkPurchase = (data) =>
  api.post("/bulkPurchase/decline", data)

export const confirmBulkPurchase = (data) =>
  api.post("/bulkPurchase/confirm", data)
