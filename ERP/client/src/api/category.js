import api from "./api";

export const fetchEquipmentCategories = () => api.get("/equipmentcategory").then((res) => res.data);

export const fetchEquipmentCategory = (equipmentCategoryId) => api.get(`/equipmentcategory/${equipmentCategoryId}`).then((res) => res.data);
