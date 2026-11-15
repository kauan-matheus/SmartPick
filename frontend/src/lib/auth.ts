export interface AuthResponse {
  token: string;
  expiresIn: string;
  usuario: {
    id: string;
    nome: string;
    email: string;
    type: string;
  };
}

const apiBaseUrl =
  process.env.NEXT_PUBLIC_API_URL?.replace(/\/+$/, "") ??
  "https://localhost:7004";

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === "object" && value !== null;
}

function isAuthResponse(value: unknown): value is AuthResponse {
  if (!isRecord(value) || !isRecord(value.usuario)) {
    return false;
  }

  return (
    typeof value.token === "string" &&
    typeof value.expiresIn === "string" &&
    typeof value.usuario.id === "string" &&
    typeof value.usuario.nome === "string" &&
    typeof value.usuario.email === "string" &&
    typeof value.usuario.type === "string"
  );
}

//Criado um login com variaveis fixas, futuramente alterar para variaveis estaticas
export async function login(
  username: string,
  password: string,
): Promise<AuthResponse> {
  if (process.env.NODE_ENV === "development") {
    if (username.trim() !== "usuario") {
      throw new Error("Nome de usuário incorreto, tente novamente!");
    }

    if (password !== "1234") {
      throw new Error("Senha incorreta, tente novamente!");
    }

    return {
      token: "development-demo-token",
      expiresIn: "8h",
      usuario: {
        id: "demo-user",
        nome: "Usuário",
        email: "usuario",
        type: "EMPLOYEE",
      },
    };
  }

  let response: Response;

  try {
    response = await fetch(`${apiBaseUrl}/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email: username, password }),
    });
  } catch {
    throw new Error(
      "Não foi possível conectar ao servidor. Verifique se a API está em execução.",
    );
  }

  if (!response.ok) {
    const body = await response.text();
    let message = "E-mail ou senha incorretos.";

    if (body) {
      try {
        const parsed: unknown = JSON.parse(body);
        if (typeof parsed === "string") {
          message = parsed;
        } else if (
          parsed &&
          typeof parsed === "object" &&
          "message" in parsed &&
          typeof parsed.message === "string"
        ) {
          message = parsed.message;
        }
      } catch {
        message = body;
      }
    }

    throw new Error(message);
  }

  let payload: unknown;

  try {
    payload = await response.json();
  } catch {
    throw new Error("O servidor retornou uma resposta inválida.");
  }

  if (!isAuthResponse(payload)) {
    throw new Error("O servidor retornou dados de acesso incompletos.");
  }

  return payload;
}
