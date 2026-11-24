import { api } from "@/lib/api";
import type { Order, OrderTaskSummary } from "@/lib/orders";

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

interface ApiTaskInPath {
  id: string;
  description: string;
  quantity: number;
  position: { x: number; y: number };
  productId: string;
}

interface ApiPickingPlan {
  lightTasks: ApiTaskInPath[];
  heavyTasks: ApiTaskInPath[];
}

export interface PickingPlan {
  light: PickingStop[];
  heavy: PickingStop[];
}

function toStops(
  tasks: ApiTaskInPath[],
  orderItems: OrderTaskSummary[],
  requiresForklift: boolean,
): PickingStop[] {
  const itemsByTaskId = new Map(orderItems.map((item) => [item.id.toLowerCase(), item]));

  return tasks.map((task) => {
    const item = itemsByTaskId.get(task.id.toLowerCase());
    const { x, y } = task.position;

    return {
      id: task.id,
      address: `(${x}, ${y})`,
      location: `Coordenadas do endereço · X ${x} · Y ${y}`,
      name: item?.name || task.description || "Produto",
      detail: item?.detail ?? "",
      quantity: task.quantity,
      requiresForklift,
    };
  });
}

export async function getPickingPlan(order: Order): Promise<PickingPlan> {
  const { data } = await api.post<ApiPickingPlan>(
    `/api/PathFind/${encodeURIComponent(order.id)}`,
  );

  return {
    light: toStops(data.lightTasks ?? [], order.lightItems, false),
    heavy: toStops(data.heavyTasks ?? [], order.heavyItems, true),
  };
}
