import type { AuthResponse } from "@/lib/auth";

export const authStorageKey = "smartpick.auth";

function isAuthResponse(value: unknown): value is AuthResponse {
  if (
    typeof value !== "object" ||
    value === null ||
    !("token" in value) ||
    !("expiresIn" in value) ||
    !("usuario" in value)
  ) {
    return false;
  }

  const usuario = value.usuario;

  return (
    typeof value.token === "string" &&
    value.token.length > 0 &&
    typeof value.expiresIn === "string" &&
    typeof usuario === "object" &&
    usuario !== null &&
    "id" in usuario &&
    typeof usuario.id === "string" &&
    "nome" in usuario &&
    typeof usuario.nome === "string" &&
    "email" in usuario &&
    typeof usuario.email === "string" &&
    "type" in usuario &&
    typeof usuario.type === "string"
  );
}

export function getStoredAuth(): AuthResponse | null {
  const storedAuth = sessionStorage.getItem(authStorageKey);

  if (!storedAuth) {
    return null;
  }

  try {
    const auth: unknown = JSON.parse(storedAuth);
    if (isAuthResponse(auth)) {
      return auth;
    }
  } catch {
    sessionStorage.removeItem(authStorageKey);
    return null;
  }

  sessionStorage.removeItem(authStorageKey);
  return null;
}

export function clearStoredAuth() {
  sessionStorage.removeItem(authStorageKey);
}
