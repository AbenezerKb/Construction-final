import api from "./api";

export const uploadFileAPI = (data) => (api.post("/file/upload", data)).then((res) => res.data);
