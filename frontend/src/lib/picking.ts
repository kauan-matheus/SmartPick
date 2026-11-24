export type TaskType = "light" | "heavy";

export interface PickingStop {
  id: string;
  address: string;
  location: string;
  name: string;
  detail: string;
  quantity: number;
  requiresForklift: boolean;
}

const heavyStops: PickingStop[] = [
  { id: "A1", address: "A1", location: "Corredor A · Posição 1 · Térreo", name: "Geladeiras", detail: "Frost Free 375 L · SKU GEL-375", quantity: 3, requiresForklift: true },
  { id: "A2", address: "A2", location: "Corredor A · Posição 2 · Térreo", name: "Fogões", detail: "5 bocas inox · SKU FOG-5B", quantity: 2, requiresForklift: true },
  { id: "B1", address: "B1", location: "Corredor B · Posição 1 · Nível 1", name: "Máquinas de lavar", detail: "Lavadora 12 kg · SKU LAV-12", quantity: 2, requiresForklift: true },
  { id: "B2", address: "B2", location: "Corredor B · Posição 2 · Nível 1", name: "Freezers", detail: "Horizontal 300 L · SKU FRZ-300", quantity: 2, requiresForklift: true },
  { id: "C1", address: "C1", location: "Corredor C · Posição 1 · Térreo", name: "Lava-louças", detail: "14 serviços · SKU LLO-14", quantity: 1, requiresForklift: true },
];

const lightStops: PickingStop[] = [
  { id: "A1", address: "A1", location: "Corredor A · Posição 1 · Prateleira 2", name: "Liquidificadores", detail: "Turbo 700 W · SKU LIQ-700", quantity: 4, requiresForklift: false },
  { id: "A2", address: "A2", location: "Corredor A · Posição 2 · Prateleira 1", name: "Cafeteiras", detail: "Elétrica 30 xícaras · SKU CAF-30", quantity: 3, requiresForklift: false },
  { id: "B1", address: "B1", location: "Corredor B · Posição 1 · Prateleira 3", name: "Ferros de passar", detail: "A vapor · SKU FER-VP", quantity: 3, requiresForklift: false },
  { id: "B2", address: "B2", location: "Corredor B · Posição 2 · Prateleira 2", name: "Sanduicheiras", detail: "Grill 750 W · SKU SAN-750", quantity: 2, requiresForklift: false },
  { id: "C1", address: "C1", location: "Corredor C · Posição 1 · Prateleira 1", name: "Batedeiras", detail: "Planetária 5 L · SKU BAT-5L", quantity: 2, requiresForklift: false },
];

export function getPickingStops(
  taskType: TaskType,
  expectedQuantity: number
): PickingStop[] {
  if (!Number.isInteger(expectedQuantity) || expectedQuantity < 0) {
    throw new RangeError("A quantidade da coleta deve ser um inteiro não negativo.");
  }

  const stops = taskType === "heavy" ? heavyStops : lightStops;
  const totalQuantity = stops.reduce((sum, stop) => sum + stop.quantity, 0);
  const exactQuantities = stops.map(
    (stop) => (expectedQuantity * stop.quantity) / totalQuantity
  );
  const quantities = exactQuantities.map(Math.floor);
  let remainder =
    expectedQuantity - quantities.reduce((sum, quantity) => sum + quantity, 0);
  const remainderOrder = exactQuantities
    .map((quantity, index) => ({
      index,
      fraction: quantity - Math.floor(quantity),
    }))
    .sort((a, b) => b.fraction - a.fraction || a.index - b.index);

  for (let index = 0; remainder > 0; index++, remainder--) {
    quantities[remainderOrder[index % remainderOrder.length].index]++;
  }

  return stops.flatMap((stop, index) =>
    quantities[index] > 0 ? [{ ...stop, quantity: quantities[index] }] : []
  );
}