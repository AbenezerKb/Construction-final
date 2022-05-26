import api from "./api"

export const fetchRoles = () => api.get("/userRole").then((res) => res.data)

export const fetchRole = (id) =>
  api.get(`/userRole/${id}`).then((res) => res.data)

export const addRole = (data) => api.post("/userRole/add", data)

export const editRole = (data) => api.post("/userRole/edit", data)
