"use client";

import { useState } from "react";
import { ArrowDownWideNarrow, Search } from "lucide-react";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { OrderCard } from "@/components/order-card";
import { orders, type Order } from "@/lib/orders";
import { cn } from "@/lib/utils";

type Filter = "todos" | "abertos" | "alerta" | "concluidos";
type SortOrder =
  | "priority"
  | "completion"
  | "id-ascending"
  | "id-descending";

function isSortOrder(value: string): value is SortOrder {
  return (
    value === "priority" ||
    value === "completion" ||
    value === "id-ascending" ||
    value === "id-descending"
  );
}

const priority: Record<Order["status"], number> = {
  alerta: 0,
  aguardando: 1,
  concluido: 2,
};

function matchesFilter(order: Order, filter: Filter) {
  switch (filter) {
    case "todos":
      return true;
    case "abertos":
      return order.status !== "concluido";
    case "alerta":
      return order.status === "alerta";
    case "concluidos":
      return order.status === "concluido";
  }
}

export function OrdersScreen() {
  const [query, setQuery] = useState("");
  const [filter, setFilter] = useState<Filter>("abertos");
  const [sortOrder, setSortOrder] = useState<SortOrder>("priority");

  const filters: { key: Filter; label: string }[] = [
    {
      key: "todos",
      label: `Todos · ${orders.length}`,
    },
    {
      key: "abertos",
      label: `Em aberto · ${orders.filter((o) => matchesFilter(o, "abertos")).length}`,
    },
    {
      key: "alerta",
      label: `Com alerta · ${orders.filter((o) => matchesFilter(o, "alerta")).length}`,
    },
    { key: "concluidos", label: "Concluídos" },
  ];

  const visible = orders
    .filter((o) => matchesFilter(o, filter))
    .filter((o) =>
      `${o.id} ${o.category} ${o.recipient}`
        .toLowerCase()
        .includes(query.toLowerCase())
    )
    .sort((a, b) => {
      switch (sortOrder) {
        case "id-ascending":
          return a.id - b.id;
        case "id-descending":
          return b.id - a.id;
        case "completion":
          return (
            Number(b.status === "concluido") -
              Number(a.status === "concluido") ||
            priority[a.status] - priority[b.status]
          );
        case "priority":
          return priority[a.status] - priority[b.status];
      }
    });

  const countLabel =
    filter === "todos"
      ? "pedidos"
      : filter === "concluidos"
        ? "pedidos concluídos"
        : "pedidos em aberto";

  return (
    <main className="mx-auto w-full max-w-6xl flex-1 px-4 pb-28 pt-5 md:px-8 md:pb-10 md:pt-8">
      {/* Título + avatar */}
      <div className="flex items-start justify-between">
        <div>
          <h1 className="text-2xl font-bold md:text-3xl">Pedidos</h1>
          <p className="mt-1 text-sm text-neutral-500">
            Bom trabalho, João · Turno da manhã
          </p>
        </div>
        <Avatar className="size-10 bg-orange-soft">
          <AvatarFallback className="bg-orange-soft text-sm font-semibold text-orange">
            JS
          </AvatarFallback>
        </Avatar>
      </div>

      {/* Busca */}
      <div className="relative mt-5 md:max-w-md">
        <Search className="absolute left-3 top-1/2 size-4 -translate-y-1/2 text-neutral-500" />
        <Input
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Buscar pedido ou destinatário"
          className="h-11 rounded-xl bg-white pl-9"
        />
      </div>

      {/* Filtros */}
      <div className="mt-4 flex flex-wrap gap-2">
        {filters.map(({ key, label }) => (
          <Button
            key={key}
            size="sm"
            variant="ghost"
            onClick={() => setFilter(key)}
            className={cn(
              "h-7 rounded-full px-3 text-xs",
              filter === key
                ? "bg-orange-soft font-semibold text-orange hover:bg-orange-soft"
                : "bg-neutral-200/60 text-neutral-600"
            )}
          >
            {label}
          </Button>
        ))}
      </div>

      {/* Contagem + ordenação */}
      <div className="mt-4 flex items-center justify-between text-xs text-neutral-500">
        <span>
          {visible.length} {countLabel}
        </span>
        <label className="flex items-center gap-1">
          <ArrowDownWideNarrow className="size-3.5" />
          <span className="sr-only">Ordenar pedidos</span>
          <select
            aria-label="Ordenar pedidos"
            value={sortOrder}
            onChange={(event) => {
              if (isSortOrder(event.target.value)) {
                setSortOrder(event.target.value);
              }
            }}
            className="cursor-pointer rounded-md border-0 bg-transparent py-1 text-xs text-neutral-500 outline-none focus-visible:ring-2 focus-visible:ring-orange"
          >
            <option value="priority">Por prioridade</option>
            <option value="completion">Por conclusão</option>
            <option value="id-ascending">Pedidos crescentes</option>
            <option value="id-descending">Pedidos decrescentes</option>
          </select>
        </label>
      </div>

      {/* Lista: 1 coluna no celular, 2 no tablet, 3 no desktop */}
      <div className="mt-3 grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        {visible.map((order) => (
          <OrderCard key={order.id} order={order} />
        ))}
      </div>

      {visible.length === 0 && (
        <p className="mt-10 text-center text-sm text-neutral-500">
          Nenhum pedido encontrado.
        </p>
      )}
    </main>
  );
}