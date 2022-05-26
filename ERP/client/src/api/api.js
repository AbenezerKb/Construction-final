import axios from "axios";

axios.defaults.headers.common["Authorization"] = "bearer " + localStorage.getItem("token");

export const url = "https://localhost:7067/api/";

export default axios.create({ baseURL: url });
