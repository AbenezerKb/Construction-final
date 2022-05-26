import api from "./api";

export const fetchAllNotifications = (data) => api.get("/notification/all").then((res) => res.data);

export const fetchNotifications = (data) => api.get("/notification").then((res) => res.data);

export const clearNotification = (notificationId) =>
    api.get(`/notification/clear/${notificationId}`);

export const fetchNow = () => api.get("/notification/now");
