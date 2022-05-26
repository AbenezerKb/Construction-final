import api from "./api"

export const fetchSites = () => api.get("/site").then((res) => res.data)

export const fetchSite = (id) => api.get(`/site/${id}`).then((res) => res.data)

export const addSite = (data) => api.post("/site", data).then((res) => res.data)

export const editSite = (data) =>
  api.post("/site/edit", data).then((res) => res.data)
