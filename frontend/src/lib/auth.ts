import axios from "axios";
import { api } from "@/lib/api";
import { enumName, userTypeNames } from "@/lib/enums";

export interface AuthResponse {
  token: string;
  expiresIn: string;
  usuario: { id: string; nome: string; email: string; type: string };
}

// Como a API realmente responde: type pode vir como número
interface LoginApiResponse {
  token: string;
  expiresIn: string;
  usuario: { id: string; nome: string; email: string; type: number | string };
}

export function getErrorMessage(error: unknown, fallback: string): string {
  if (axios.isAxiosError(error)) {
    if (!error.response) {
      return "Não foi possível conectar ao servidor. Verifique se a API está em execução.";
    }
    const data: unknown = error.response.data;
    if (typeof data === "string" && data) return data;              // ex.: 401 do login
    if (Array.isArray(data) && data.length) return data.join(" ");  // erros de validação
  }
  return fallback;
}

export async function login(
  username: string,
  password: string,
): Promise<AuthResponse> {
  try {
    const { data } = await api.post<LoginApiResponse>("/login", {
      email: username,
      password,
    });

    return {
      ...data,
      usuario: {
        ...data.usuario,
        type: enumName(userTypeNames, data.usuario.type), // 0 -> "MANAGER", 1 -> "EMPLOYEE"
      },
    };
  } catch (error) {
    throw new Error(getErrorMessage(error, "E-mail ou senha incorretos."));
  }
}