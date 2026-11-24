// A API serializa enums como número: o índice do valor no enum do C#.
// A ordem abaixo precisa ser a mesma dos enums do backend.
export const userTypeNames = ["MANAGER", "EMPLOYEE"] as const;
export const orderStatusNames = ["WAITING", "FINISHED", "CANCELED"] as const;
export const productAccessNames = ["LIGHT", "HEAVY"] as const;
export const requestObjectiveNames = ["CANCELORDER"] as const;
export const requestStatusNames = ["PENDENTE", "CONFIRMADO", "NEGADO"] as const;

// Aceita número ou string, então continua funcionando se o backend mudar um dia.
export function enumName<T extends readonly string[]>(
  names: T,
  value: number | string,
): T[number] {
  return typeof value === "string" ? (value as T[number]) : names[value];
}