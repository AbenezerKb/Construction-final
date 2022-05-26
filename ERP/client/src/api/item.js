import api from "./api"

export const fetchMaterials = () =>
  api.get("/item/material").then((res) => res.data)

export const fetchTransferableMaterials = () =>
  api.get("/item/material/transferable").then((res) => res.data)

export const fetchEquipments = () =>
  api.get("/item/equipment").then((res) => res.data)

export const fetchMinimumStockItems = (data) =>
  api.post("/item/minimum-stock", data).then((res) => res.data)

export const fetchEquipmentCategories = () =>
  api.get("/item/equipmentcategory").then((res) => res.data)

export const fetchEquipmentAssets = (data) =>
    api.get(`/item/asset?modelId=${data}`).then((res) => res.data);
    
export const fetchCleanEquipmentAssets = (data) =>
api.get(`/item/asset-clean?modelId=${data}`).then((res) => res.data);

export const fetchItem = (itemId) => api.get(`/item/${itemId}`).then((res) => res.data);

export const fetchQty = (data) =>
  api.post("/item/qty", data).then((res) => res.data)

export const fetchEquipmentModel = (modelId) =>
  api.get(`/item/equipmentmodel/${modelId}`).then((res) => res.data)

export const fetchEquipmentModels = () =>
  api.get("/item/equipmentmodels").then((res) => res.data)

export const editMaterial = (data) => api.post("/item/material/edit", data)

export const editEquipment = (data) => api.post("/item/equipment/edit", data)

export const editEquipmentModel = (data) =>
  api.post("/item/equipmentmodel/edit", data)

export const addMaterial = (data) => api.post("/item/material/add", data)

export const addEquipment = (data) => api.post("/item/equipment/add", data)

export const addEquipmentCategory = (data) =>
  api.post("/item/equipmentcategory/add", data)

export const addEquipmentModel = (data) =>
  api.post("/item/equipmentmodel/add", data)

export const updateMinimumStockItems = (data) =>
  api.post("/item/minimum-stock/update", data).then((res) => res.data)

export const importAssets = (data) =>
  api.post("/item/import-asset", data).then((res) => res.data)
