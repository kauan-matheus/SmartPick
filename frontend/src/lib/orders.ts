export type OrderStatus = "alerta" | "aguardando" | "concluido";

export interface Order {
  id: number;
  category: string;
  recipient: string;
  carrier: string;
  pickup: string;
  lightTasks: number;
  heavyTasks: number;
  units: number;
  status: OrderStatus;
}

type OrderWithoutUnitTotal = Omit<Order, "units">;

// TODO(task-counts): Aqui é onde fica a exibição dos valores estáticos da tela de Tarefas Designadas.
const orderTaskCounts: OrderWithoutUnitTotal[] = [
  {
    id: 1048,
    category: "Eletrodomésticos",
    recipient: "Loja Nova Casa · Guarulhos",
    carrier: "TransRápido",
    pickup: "11h30",
    lightTasks: 18,
    heavyTasks: 12,
    status: "alerta",
  },
  {
    id: 1049,
    category: "Utilidades domésticas",
    recipient: "Casa & Cia · São Paulo",
    carrier: "Expresso Sul",
    pickup: "14h",
    lightTasks: 12,
    heavyTasks: 6,
    status: "aguardando",
  },
  {
    id: 1050,
    category: "Móveis planejados",
    recipient: "Mobiliar · Osasco",
    carrier: "TransRápido",
    pickup: "15h",
    lightTasks: 8,
    heavyTasks: 4,
    status: "aguardando",
  },
  {
    id: 1051,
    category: "Eletrônicos",
    recipient: "Tech Center · Santo André",
    carrier: "Rota Norte",
    pickup: "10h",
    lightTasks: 30,
    heavyTasks: 15,
    status: "alerta",
  },
  {
    id: 1052,
    category: "Cama, mesa e banho",
    recipient: "Lar Doce Lar · Campinas",
    carrier: "Expresso Sul",
    pickup: "9h",
    lightTasks: 40,
    heavyTasks: 20,
    status: "concluido",
  },
];

export const orders: Order[] = orderTaskCounts.map((order) => ({
  ...order,
  units: order.lightTasks + order.heavyTasks,
}));