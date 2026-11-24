"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import {
  ArrowLeft,
  ArrowRight,
  Check,

  Forklift,
  Package,
  PackageOpen,

} from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import type { Order } from "@/lib/orders";

interface AssignedTaskDetailsProps {
  order: Order;
}

type TaskType = "light" | "heavy";

export function AssignedTaskDetails({ order }: AssignedTaskDetailsProps) {
  const router = useRouter();
  const [selectedTaskType, setSelectedTaskType] = useState<TaskType | null>(null);
  const completed = order.status === "concluido";
  const statusLabel = completed ? "Concluída" : "Não iniciada";

  return (
    <main className="w-full flex-1 px-3 pb-8 pt-4 sm:px-4 sm:pb-10 sm:pt-5 md:px-8 md:pt-8">
      <div className="-mx-3 -mt-4 bg-white px-3 py-3 sm:-mx-4 sm:-mt-5 sm:px-4 sm:py-4 md:-mx-8 md:-mt-8 md:px-8 md:py-6">
        <div className="flex items-center gap-3">
          <Link
            href="/"
            aria-label="Voltar para pedidos"
            className="flex size-10 shrink-0 items-center justify-center rounded-xl border bg-white text-neutral-700 hover:bg-neutral-100"
          >
            <ArrowLeft className="size-5" />
          </Link>
          <div>
            <h1 className="text-[clamp(1.125rem,3.2vw,1.5rem)] font-bold">
              Tarefas designadas
            </h1>
            <p className="mt-1 text-sm text-neutral-500">
              Sua fila de trabalho
            </p>
          </div>
        </div>
      </div>

      <section className="mt-4 sm:mt-5" aria-labelledby="order-summary-title">
        <div className="flex items-start justify-between gap-3">
          <div className="min-w-0">
            <h2 id="order-summary-title" className="font-semibold">
              Pedido #{order.id}
            </h2>
            <p className="mt-1 text-sm text-neutral-500">
              {order.category} · {statusLabel}
            </p>
          </div>
          <Package className="mt-1 size-5 shrink-0 text-orange" />
        </div>
      </section>

      <section className="mt-5 sm:mt-6" aria-labelledby="task-type-title">
        <div className="mb-3 flex flex-wrap items-center justify-between gap-2">
          <h2
            id="task-type-title"
            className="min-w-0 flex-1 text-[clamp(0.9rem,2.8vw,1rem)] font-semibold"
          >
            Selecione uma tarefa para começar a coleta
          </h2>
          <Badge
            variant="secondary"
            className={`shrink-0 ${
              order.status === "alerta"
                ? "bg-warn-bg text-warn-fg"
                : "bg-muted-bg text-neutral-600"
            }`}
          >
            {order.status === "alerta" ? "Prioridade alta" : "Prioridade normal"}
          </Badge>
        </div>

        <div className="grid grid-cols-2 gap-3">
          <Button
            type="button"
            variant="outline"
            aria-pressed={selectedTaskType === "light"}
            disabled={completed || order.lightTasks === 0}
            onClick={() => setSelectedTaskType("light")}
            className={`h-auto min-h-28 flex-col items-start justify-between rounded-2xl p-3 text-left whitespace-normal sm:min-h-32 sm:p-4 ${
              selectedTaskType === "light"
                ? "border-orange bg-orange-soft text-foreground"
                : "bg-white"
            }`}
          >
            <span className="flex w-full items-center justify-between gap-2">
              <PackageOpen className="size-5 text-orange" />
              {selectedTaskType === "light" && <Check className="size-4 text-orange" />}
            </span>
            <span>
              <span className="block text-[clamp(1.5rem,4vw,2rem)] font-bold">{order.lightTasks}</span>
              <span className="mt-1 block text-sm font-semibold">Tarefas leves</span>
              <span className="mt-1 block text-xs text-neutral-500">
                {order.lightTasks === 0
                  ? "Não é possível selecionar: não existem tarefas."
                  : selectedTaskType === "light"
                    ? "Selecionada"
                    : "Toque para selecionar"}
              </span>
            </span>
          </Button>

          <Button
            type="button"
            variant="outline"
            aria-pressed={selectedTaskType === "heavy"}
            disabled={completed || order.heavyTasks === 0}
            onClick={() => setSelectedTaskType("heavy")}
            className={`h-auto min-h-28 flex-col items-start justify-between rounded-2xl p-3 text-left whitespace-normal sm:min-h-32 sm:p-4 ${
              selectedTaskType === "heavy"
                ? "border-orange bg-orange-soft text-foreground"
                : "bg-white"
            }`}
          >
            <span className="flex w-full items-center justify-between gap-2">
              <Forklift className="size-5 text-orange" />
              {selectedTaskType === "heavy" && <Check className="size-4 text-orange" />}
            </span>
            <span>
              <span className="block text-[clamp(1.5rem,4vw,2rem)] font-bold">{order.heavyTasks}</span>
              <span className="mt-1 block text-sm font-semibold">Tarefas pesadas</span>
              <span className="mt-1 block text-xs text-neutral-500">
                {order.heavyTasks === 0
                  ? "Não é possível selecionar: não existem tarefas."
                  : selectedTaskType === "heavy"
                    ? "Selecionada"
                    : "Toque para selecionar"}
              </span>
            </span>
          </Button>
        </div>

        {selectedTaskType === "heavy" && (
          <Badge
            variant="secondary"
            className="mt-3 h-auto w-full justify-start rounded-lg bg-warn-bg px-3 py-2 text-left whitespace-normal text-warn-fg"
          >
            Será necessário o uso de uma empilhadeira para este tipo de Tarefa!
          </Badge>
        )}

        <Card className="mt-4 gap-0 rounded-2xl p-3 shadow-sm sm:p-4">
          {selectedTaskType ? (
            <>
              {(() => {
                const items = selectedTaskType === "light" ? order.lightItems : order.heavyItems;
                const unitCount = selectedTaskType === "light" ? order.lightTasks : order.heavyTasks;

                return (
                  <>
                    <div className="flex items-center justify-between gap-3">
                      <h3 className="font-semibold">
                        Resumo da coleta {selectedTaskType === "light" ? "leve" : "pesada"}
                      </h3>
                      <span className="shrink-0 text-xs text-neutral-500">
                        {items.length} {items.length === 1 ? "item" : "itens"} · {unitCount} un.
                      </span>
                    </div>
                    {items.length === 0 ? (
                      <p className="mt-3 text-sm text-neutral-500">
                        Nao existem tarefas neste grupo.
                      </p>
                    ) : (
                      <ul className="mt-3 max-h-56 divide-y overflow-y-auto border-t text-sm">
                        {items.map((item) => (
                          <li key={item.id} className="flex items-start justify-between gap-3 py-3">
                            <span className="min-w-0">
                              <span className="block break-words font-medium">{item.name}</span>
                              {item.detail && (
                                <span className="mt-0.5 block break-words text-xs text-neutral-500">
                                  {item.detail}
                                </span>
                              )}
                            </span>
                            <span className="shrink-0 font-semibold">{item.quantity} un.</span>
                          </li>
                        ))}
                      </ul>
                    )}
                  </>
                );
              })()}
            </>
          ) : (
            <p className="py-2 text-sm text-neutral-500">
              Selecione um grupo para ver os itens da coleta.
            </p>
          )}
        </Card>

        <Button
          type="button"
          disabled={completed || selectedTaskType === null}
          onClick={() =>
            router.push(`/coleta-guiada/${order.id}?tipo=${selectedTaskType}`)
          }
          className="mt-4 h-12 w-full rounded-xl bg-orange text-base font-semibold text-white hover:bg-orange/90"
        >
          {completed ? (
            <>
              <Check className="size-5" />
              Coleta concluída
            </>
          ) : (
            <>
              <ArrowRight className="size-5" />
              Iniciar coleta
            </>
          )}
        </Button>
      </section>
    </main>
  );
}
