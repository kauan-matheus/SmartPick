import { api } from "@/lib/api";
import { enumName, orderStatusNames, productAccessNames } from "@/lib/enums";

export type OrderStatus = "alerta" | "aguardando" | "concluido";

export interface OrderTaskSummary {
  id: string;
  name: string;
  detail: string;
  quantity: number;
}

export interface Order {
  id: string; // Guid da API, usado nas rotas e nas chamadas
  code: string; // versão curta, só para exibir
  category: string;
  recipient: string;
  carrier: string;
  pickup: string;
  pickupAt: number; // timestamp, usado para ordenar
  lightTasks: number;
  heavyTasks: number;
  lightItems: OrderTaskSummary[];
  heavyItems: OrderTaskSummary[];
  units: number;
  status: OrderStatus;
}

// Formato real que a API devolve
interface ApiTask {
  id: string;
  description: string;
  status: number | string;
  quantity: number;
  productId: string;
}
interface ApiOrder {
  id: string;
  description: string;
  destinatario: string;
  transportadora: string;
  totalProdutos: number;
  dataColeta: string;
  tasks: ApiTask[];
  status: number | string;
}
interface ApiProduct {
  id: string;
  name: string;
  access: number | string;
}

// Pedido aguardando com coleta em até 2h (ou atrasada) vira "alerta"
const ALERT_WINDOW_MS = 2 * 60 * 60 * 1000;

function formatPickup(iso: string) {
  const date = new Date(iso);
  const hours = date.getHours();
  const minutes = date.getMinutes();
  return minutes === 0 ? `${hours}h` : `${hours}h${String(minutes).padStart(2, "0")}`;
}

function productAccessName(value: number | string): string {
  if (typeof value === "number") return productAccessNames[value] ?? "";

  const normalized = value.trim().toUpperCase();
  if (normalized === "0") return "LIGHT";
  if (normalized === "1") return "HEAVY";
  return normalized;
}

function toOrder(
  order: ApiOrder,
  productsById: Map<string, ApiProduct>,
): Order | null {
  const apiStatus = enumName(orderStatusNames, order.status);
  if (apiStatus === "CANCELED") return null;

  let lightTasks = 0;
  let heavyTasks = 0;
  const lightItems: OrderTaskSummary[] = [];
  const heavyItems: OrderTaskSummary[] = [];
  for (const task of order.tasks ?? []) {
    const product = productsById.get(task.productId.toLowerCase());
    const item: OrderTaskSummary = {
      id: task.id,
      name: product?.name || task.description || "Item sem nome",
      detail:
        task.description && task.description !== product?.name
          ? task.description
          : "",
      quantity: task.quantity,
    };

    if (product && productAccessName(product.access) === "HEAVY") {
      heavyTasks += task.quantity;
      heavyItems.push(item);
    } else {
      lightTasks += task.quantity;
      lightItems.push(item);
    }
  }

  const pickupAt = new Date(order.dataColeta).getTime();
  let status: OrderStatus = "aguardando";
  if (apiStatus === "FINISHED") status = "concluido";
  else if (pickupAt - Date.now() <= ALERT_WINDOW_MS) status = "alerta";

  return {
    id: order.id,
    code: order.id.slice(0, 8).toUpperCase(),
    category: order.description,
    recipient: order.destinatario,
    carrier: order.transportadora,
    pickup: formatPickup(order.dataColeta),
    pickupAt,
    lightTasks,
    heavyTasks,
    lightItems,
    heavyItems,
    units: lightTasks + heavyTasks,
    status,
  };
}

export async function getOrders(): Promise<Order[]> {
  // As duas chamadas são independentes, então rodam em paralelo
  const [ordersRes, productsRes] = await Promise.all([
    api.get<ApiOrder[]>("/api/Order"),
    api.get<ApiProduct[]>("/api/Product"),
  ]);

  const productsById = new Map<string, ApiProduct>(
    productsRes.data.map((product) => [product.id.toLowerCase(), product]),
  );

  return ordersRes.data.flatMap((order) => {
    const mapped = toOrder(order, productsById);
    return mapped ? [mapped] : [];
  });
}

export async function getOrderById(id: string): Promise<Order | null> {
  const [orderRes, tasksRes, productsRes] = await Promise.all([
    api.get<ApiOrder>(`/api/Order/${encodeURIComponent(id)}`),
    api.get<ApiTask[]>(`/api/Order/Tarefas/${encodeURIComponent(id)}`),
    api.get<ApiProduct[]>("/api/Product"),
  ]);

  const productsById = new Map<string, ApiProduct>(
    productsRes.data.map((product) => [
      product.id.toLowerCase(),
      product,
    ]),
  );

  return toOrder(
    { ...orderRes.data, tasks: tasksRes.data },
    productsById,
  );
}
