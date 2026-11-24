import axios from "axios";
import { clearSession, getStoredAuth } from "@/lib/auth-session";

export const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL?.replace(/\/+$/, "") ?? "http://localhost:5192",
  timeout: 15000,
});

api.interceptors.request.use((config) => {
  const token = getStoredAuth()?.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (res) => res,
  (error) => {
    const isLogin = error.config?.url === "/login";
    if (axios.isAxiosError(error) && error.response?.status === 401 && !isLogin) {
      clearSession();
      window.location.replace("/login");
    }
    return Promise.reject(error);
  },
);