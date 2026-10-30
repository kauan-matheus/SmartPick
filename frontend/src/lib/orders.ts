export type OrderStatus = "alerta" | "aguardando" | "concluido";

export interface Order {
  id: number;
  category: string;
  recipient: string;
  carrier: string;
  pickup: string;
  units: number;
  status: OrderStatus;
}

export const orders: Order[] = [
  {
    id: 1048,
    category: "Eletrodomésticos",
    recipient: "Loja Nova Casa · Guarulhos",
    carrier: "TransRápido",
    pickup: "11h30",
    units: 30,
    status: "alerta",
  },
  {
    id: 1049,
    category: "Utilidades domésticas",
    recipient: "Casa & Cia · São Paulo",
    carrier: "Expresso Sul",
    pickup: "14h",
    units: 18,
    status: "aguardando",
  },
  {
    id: 1050,
    category: "Móveis planejados",
    recipient: "Mobiliar · Osasco",
    carrier: "TransRápido",
    pickup: "15h",
    units: 12,
    status: "aguardando",
  },
  {
    id: 1051,
    category: "Eletrônicos",
    recipient: "Tech Center · Santo André",
    carrier: "Rota Norte",
    pickup: "10h",
    units: 45,
    status: "alerta",
  },
  {
    id: 1052,
    category: "Cama, mesa e banho",
    recipient: "Lar Doce Lar · Campinas",
    carrier: "Expresso Sul",
    pickup: "9h",
    units: 60,
    status: "concluido",
  },
];