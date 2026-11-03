"use client";

import Link from "next/link";
import { useState } from "react";
import { ArrowDownWideNarrow, Search } from "lucide-react";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { OrderCard } from "@/components/order-card";
import {
  saveTarefasDesignadasID,
  useTarefasDesignasID,
} from "@/lib/assigned-tasks";
import { orders, type Order } from "@/lib/orders";
import { cn } from "@/lib/utils";

type Filter = "todas" | "alerta" | "concluidas";

const priority: Record<Order["status"], number> = {
  alerta: 0,
  aguardando: 1,
  concluido: 2,
};

function matchesFilter(order: Order, filter: Filter) {
  switch (filter) {
    case "todas":
      return true;
    case "alerta":
      return order.status === "alerta";
    case "concluidas":
      return order.status === "concluido";
  }
}

export function MyTasksScreen() {
  const [query, setQuery] = useState("");
  const [filter, setFilter] = useState<Filter>("todas");
  const tarefasAtribuidasIds = useTarefasDesignasID();

  function removeTask(orderId: number) {
    const updatedIds = tarefasAtribuidasIds.filter((id) => id !== orderId);
    saveTarefasDesignadasID(updatedIds);
  }

  const pedidosAtribuidos = orders.filter((order) =>
    tarefasAtribuidasIds.includes(order.id)
  );
  const filters: { key: Filter; label: string }[] = [
    { key: "todas", label: `Atribuídas · ${pedidosAtribuidos.length}` },
    {
      key: "alerta",
      label: `Com alerta · ${
        pedidosAtribuidos.filter((order) => order.status === "alerta").length
      }`,
    },
    {
      key: "concluidas",
      label: `Concluídas · ${
        pedidosAtribuidos.filter((order) => order.status === "concluido").length
      }`,
    },
  ];

  const visible = pedidosAtribuidos
    .filter((order) => matchesFilter(order, filter))
    .filter((order) =>
      `${order.id} ${order.category} ${order.recipient}`
        .toLowerCase()
        .includes(query.toLowerCase())
    )
    .sort((a, b) => priority[a.status] - priority[b.status]);

  return (
    <main className="mx-auto w-full max-w-6xl flex-1 px-4 pb-28 pt-5 md:px-8 md:pb-10 md:pt-8">
      <div className="flex items-start justify-between">
        <div>
          <h1 className="text-2xl font-bold md:text-3xl">Tarefas Designadas</h1>
          <p className="mt-1 text-sm text-neutral-500">
            Sua Fila de Trabalhos
          </p>
        </div>
        <Avatar className="size-10 bg-orange-soft">
          <AvatarFallback className="bg-orange-soft text-sm font-semibold text-orange">
            JS
          </AvatarFallback>
        </Avatar>
      </div>

      <div className="relative mt-5 md:max-w-md">
        <Search className="absolute left-3 top-1/2 size-4 -translate-y-1/2 text-neutral-500" />
        <Input
          value={query}
          onChange={(event) => setQuery(event.target.value)}
          placeholder="Buscar tarefa ou destinatário"
          className="h-11 rounded-xl bg-white pl-9"
        />
      </div>

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

      <div className="mt-4 flex items-center justify-between text-xs text-neutral-500">
        <span>
          {visible.length} {visible.length === 1 ? "tarefa" : "tarefas"}
        </span>
        <span className="flex items-center gap-1">
          <ArrowDownWideNarrow className="size-3.5" />
          Por prioridade
        </span>
      </div>

      <div className="mt-3 grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        {visible.map((order) => (
          <OrderCard
            key={order.id}
            order={order}
            isAssigned
            onToggleTask={removeTask}
          />
        ))}
      </div>

      {visible.length === 0 && (
        <div className="mt-10 text-center text-sm text-neutral-500">
          {pedidosAtribuidos.length === 0 ? (
            <>
              <p>Você ainda não selecionou nenhuma tarefa.</p>
              <Link
                href="/"
                className="mt-2 inline-block font-semibold text-orange hover:underline"
              >
                Ver pedidos
              </Link>
            </>
          ) : (
            <p>Nenhuma tarefa encontrada.</p>
          )}
        </div>
      )}
    </main>
  );
}
