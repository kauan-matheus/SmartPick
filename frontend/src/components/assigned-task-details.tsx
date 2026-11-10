"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import {
  ArrowLeft,
  ArrowRight,
  Check,
  Clock3,
  Forklift,
  Package,
  PackageOpen,
  Truck,
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
    <main className="mx-auto w-full max-w-3xl flex-1 px-4 pb-10 pt-5 md:px-8 md:pb-10 md:pt-8">
      <div className="-mx-4 -mt-5 bg-white px-4 py-4 md:-mx-8 md:-mt-8 md:px-8 md:py-6">
        <div className="flex items-center gap-3">
          <Link
            href="/"
            aria-label="Voltar para pedidos"
            className="flex size-10 shrink-0 items-center justify-center rounded-xl border bg-white text-neutral-700 hover:bg-neutral-100"
          >
            <ArrowLeft className="size-5" />
          </Link>
          <div>
            <h1 className="text-xl font-bold md:text-2xl">
              Tarefas designadas
            </h1>
            <p className="mt-1 text-sm text-neutral-500">
              Sua fila de trabalho
            </p>
          </div>
        </div>
      </div>

      <section className="mt-5" aria-labelledby="order-summary-title">
        <div className="flex items-start justify-between gap-3">
          <div>
            <h2 id="order-summary-title" className="font-semibold">
              Pedido #{order.id}
            </h2>
            <p className="mt-1 text-sm text-neutral-500">
              {order.category} · {statusLabel}
            </p>
          </div>
          <Package className="mt-1 size-5 text-orange" />
        </div>
      </section>

      <section className="mt-6" aria-labelledby="task-type-title">
        <div className="mb-3 flex items-center justify-between gap-3">
          <h2 id="task-type-title" className="font-semibold">
            Selecione uma tarefa para começar a coleta
          </h2>
          <Badge
            variant="secondary"
            className={
              order.status === "alerta"
                ? "bg-warn-bg text-warn-fg"
                : "bg-muted-bg text-neutral-600"
            }
          >
            {order.status === "alerta" ? "Prioridade alta" : "Prioridade normal"}
          </Badge>
        </div>

        <div className="grid grid-cols-2 gap-3">
          <Button
            type="button"
            variant="outline"
            aria-pressed={selectedTaskType === "light"}
            disabled={completed}
            onClick={() => setSelectedTaskType("light")}
            className={`h-auto min-h-32 flex-col items-start justify-between rounded-2xl p-4 text-left whitespace-normal ${
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
              <span className="block text-2xl font-bold">{order.lightTasks}</span>
              <span className="mt-1 block text-sm font-semibold">Tarefas leves</span>
              <span className="mt-1 block text-xs text-neutral-500">
                {selectedTaskType === "light" ? "Selecionada" : "Toque para selecionar"}
              </span>
            </span>
          </Button>

          <Button
            type="button"
            variant="outline"
            aria-pressed={selectedTaskType === "heavy"}
            disabled={completed}
            onClick={() => setSelectedTaskType("heavy")}
            className={`h-auto min-h-32 flex-col items-start justify-between rounded-2xl p-4 text-left whitespace-normal ${
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
              <span className="block text-2xl font-bold">{order.heavyTasks}</span>
              <span className="mt-1 block text-sm font-semibold">Tarefas pesadas</span>
              <span className="mt-1 block text-xs text-neutral-500">
                {selectedTaskType === "heavy" ? "Selecionada" : "Toque para selecionar"}
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

        <Card className="mt-4 gap-0 rounded-2xl p-4 shadow-sm">
          <div className="flex items-start justify-between gap-3">
            <div>
              <h3 className="text-lg font-semibold">Pedido #{order.id}</h3>
              <p className="mt-1 text-sm text-neutral-500">{order.category}</p>
            </div>
            <Badge
              variant="secondary"
              className={
                completed
                  ? "bg-orange-soft text-orange"
                  : "bg-muted-bg text-neutral-600"
              }
            >
              {statusLabel}
            </Badge>
          </div>

          <dl className="mt-4 space-y-3 border-t pt-3 text-sm">
            <div className="flex justify-between gap-3">
              <dt className="text-neutral-500">Destinatário</dt>
              <dd className="text-right font-medium">{order.recipient}</dd>
            </div>
            <div className="flex justify-between gap-3">
              <dt className="text-neutral-500">Total de tarefas / unidades</dt>
              <dd className="font-medium">{order.units}</dd>
            </div>
            <div className="flex justify-between gap-3">
              <dt className="flex items-center gap-1 text-neutral-500">
                <Truck className="size-4" />
                Transportadora
              </dt>
              <dd className="text-right font-medium">{order.carrier}</dd>
            </div>
            <div className="flex justify-between gap-3">
              <dt className="flex items-center gap-1 text-neutral-500">
                <Clock3 className="size-4" />
                Horário da coleta
              </dt>
              <dd className="font-medium">{order.pickup}</dd>
            </div>
          </dl>
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