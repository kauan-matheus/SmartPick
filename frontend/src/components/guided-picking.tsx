"use client";

import { useState } from "react";
import Link from "next/link";
import {
  ArrowLeft,
  Check,
  History,
  MapPin,
  MessageSquareWarning,
  Package,
  ScanLine,
  TriangleAlert,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { getPickingStops, type TaskType } from "@/lib/picking";
import type { Order } from "@/lib/orders";
import { cn } from "@/lib/utils";

interface GuidedPickingProps {
  order: Order;
  taskType: TaskType;
}

const justifyReasons = [
  "Item não encontrado",
  "Item danificado",
  "Endereço bloqueado",
];

export function GuidedPicking({ order, taskType }: GuidedPickingProps) {
  const stops = getPickingStops(taskType);

  const [collected, setCollected] = useState<number[]>(stops.map(() => 0));
  const [justified, setJustified] = useState<Record<string, string>>({});
  const [showJustify, setShowJustify] = useState(false);
  const [showHistory, setShowHistory] = useState(false);

  const isDone = (index: number) =>
    collected[index] >= stops[index].quantity || stops[index].id in justified;

  const currentIndex = stops.findIndex((_, index) => !isDone(index));
  const finished = currentIndex === -1;
  const current = finished ? null : stops[currentIndex];

  const totalUnits = stops.reduce((sum, stop) => sum + stop.quantity, 0);
  const collectedUnits = collected.reduce((sum, value) => sum + value, 0);
  const taskCode = `${taskType === "heavy" ? "P" : "L"}07`;

  // TODO(scanner): trocar este incremento pela leitura real do código (câmera/leitor).
  function handleScan() {
    if (!current) return;
    setCollected((prev) =>
      prev.map((value, index) => (index === currentIndex ? value + 1 : value))
    );
    setShowJustify(false);
  }

  function handleJustify(reason: string) {
    if (!current) return;
    setJustified((prev) => ({ ...prev, [current.id]: reason }));
    setShowJustify(false);
  }

  const historyItems = stops
    .map((stop, index) => ({ stop, units: collected[index] }))
    .filter(({ stop, units }) => units > 0 || stop.id in justified);

  return (
    <main className="mx-auto flex w-full max-w-xl flex-1 flex-col px-4 pt-5 md:px-8 md:pt-8">
      {/* Título */}
      <div className="-mx-4 -mt-5 bg-white px-4 py-4 md:-mx-8 md:-mt-8 md:px-8 md:py-6">
        <div className="flex items-center gap-3">
          <Link
            href={`/tarefas-designadas/${order.id}`}
            aria-label="Voltar para tarefas designadas"
            className="flex size-10 shrink-0 items-center justify-center rounded-xl border bg-white text-neutral-700 hover:bg-neutral-100"
          >
            <ArrowLeft className="size-5" />
          </Link>
          <div>
            <h1 className="text-xl font-bold md:text-2xl">Coleta guiada</h1>
            <p className="mt-1 text-sm text-neutral-500">
              Tarefa {taskCode} · Pedido #{order.id}
            </p>
          </div>
        </div>
      </div>

      {/* Progresso */}
      <section className="mt-4" aria-label="Progresso da coleta">
        <div className="flex items-center justify-between text-xs">
          <span className="font-semibold">
            {finished
              ? "Todos os endereços concluídos"
              : `Endereço ${currentIndex + 1} de ${stops.length}`}
          </span>
          <span className="text-neutral-500">
            {collectedUnits} de {totalUnits} un. coletadas
          </span>
        </div>

        <ol className="mt-2 flex gap-1.5">
          {stops.map((stop, index) => (
            <li key={stop.id} className="flex-1">
              <div
                className={cn(
                  "h-1.5 rounded-full",
                  isDone(index) || index === currentIndex
                    ? "bg-orange"
                    : "bg-neutral-200"
                )}
              />
              <span
                className={cn(
                  "mt-1 block text-xs",
                  index === currentIndex
                    ? "font-semibold text-foreground"
                    : "text-neutral-500"
                )}
              >
                {stop.address}
              </span>
            </li>
          ))}
        </ol>
      </section>

      {finished ? (
        <Card className="mt-6 items-center gap-3 rounded-2xl p-8 text-center shadow-sm">
          <span className="flex size-12 items-center justify-center rounded-full bg-orange-soft">
            <Check className="size-6 text-orange" />
          </span>
          <h2 className="text-lg font-semibold">Coleta finalizada</h2>
          <p className="text-sm text-neutral-500">
            {collectedUnits} de {totalUnits} unidades coletadas
            {Object.keys(justified).length > 0 &&
              ` · ${Object.keys(justified).length} endereço(s) justificado(s)`}
            .
          </p>
        </Card>
      ) : (
        current && (
          <div className="mt-4 space-y-4">
            {current.requiresForklift && (
              <div
                role="alert"
                className="flex gap-3 rounded-xl border border-orange/30 bg-warn-bg p-3"
              >
                <TriangleAlert className="mt-0.5 size-5 shrink-0 text-orange" />
                <div className="text-sm">
                  <p className="font-semibold text-warn-fg">
                    Veículo necessário
                  </p>
                  <p className="mt-0.5 text-xs text-warn-fg">
                    Use uma empilhadeira e um operador habilitado para
                    movimentar este item.
                  </p>
                </div>
              </div>
            )}

            {/* Endereço */}
            <section
              className="rounded-2xl bg-black p-5 text-white"
              aria-label="Endereço atual"
            >
              <p className="flex items-center gap-2 text-xs font-medium tracking-wide text-white/80">
                <MapPin className="size-4" />
                VÁ ATÉ O ENDEREÇO
              </p>
              <p className="mt-3 text-6xl font-bold">{current.address}</p>
              <p className="mt-3 text-sm text-white/70">{current.location}</p>
            </section>

            {/* Item */}
            <Card className="gap-0 rounded-2xl p-4 shadow-sm">
              <div className="flex items-center gap-3">
                <span className="flex size-12 shrink-0 items-center justify-center rounded-xl border bg-neutral-100">
                  <Package className="size-6 text-neutral-700" />
                </span>
                <div>
                  <h2 className="text-xl font-bold">{current.name}</h2>
                  <p className="text-sm text-neutral-500">{current.detail}</p>
                </div>
              </div>

              <div className="mt-4 flex items-end justify-between gap-3 border-t pt-3">
                <div>
                  <p className="text-sm text-neutral-500">
                    Quantidade neste endereço
                  </p>
                  <p className="mt-1 text-sm font-semibold">
                    Leia o código de cada unidade
                  </p>
                  <p className="mt-1 text-xs text-neutral-500">
                    {collected[currentIndex]} de {current.quantity} lidas
                  </p>
                </div>
                <p className="flex items-baseline gap-1">
                  <span className="text-4xl font-bold">{current.quantity}</span>
                  <span className="text-sm text-neutral-500">un.</span>
                </p>
              </div>
            </Card>

            {/* Ações secundárias */}
            <div className="grid grid-cols-2 gap-3">
              <Button
                type="button"
                variant="outline"
                aria-expanded={showJustify}
                onClick={() => {
                  setShowJustify((value) => !value);
                  setShowHistory(false);
                }}
                className="h-11 rounded-xl bg-white font-semibold text-orange"
              >
                <MessageSquareWarning className="size-4" />
                Justificar
              </Button>
              <Button
                type="button"
                variant="outline"
                aria-expanded={showHistory}
                onClick={() => {
                  setShowHistory((value) => !value);
                  setShowJustify(false);
                }}
                className="h-11 rounded-xl bg-white font-semibold text-orange"
              >
                <History className="size-4" />
                Histórico coletados
              </Button>
            </div>

            {showJustify && (
              <Card className="gap-2 rounded-2xl p-4 shadow-sm">
                <h3 className="text-sm font-semibold">
                  Por que não foi possível coletar?
                </h3>
                {justifyReasons.map((reason) => (
                  <Button
                    key={reason}
                    type="button"
                    variant="outline"
                    onClick={() => handleJustify(reason)}
                    className="h-10 justify-start rounded-xl bg-white"
                  >
                    {reason}
                  </Button>
                ))}
              </Card>
            )}
          </div>
        )
      )}

      {showHistory && (
        <Card className="mt-4 gap-2 rounded-2xl p-4 shadow-sm">
          <h3 className="text-sm font-semibold">Histórico coletados</h3>
          {historyItems.length === 0 ? (
            <p className="text-sm text-neutral-500">
              Nenhum item coletado ainda.
            </p>
          ) : (
            <ul className="divide-y text-sm">
              {historyItems.map(({ stop, units }) => (
                <li
                  key={stop.id}
                  className="flex items-center justify-between gap-3 py-2"
                >
                  <span>
                    <span className="font-semibold">{stop.address}</span> ·{" "}
                    {stop.name}
                  </span>
                  <span className="text-right text-neutral-500">
                    {stop.id in justified
                      ? justified[stop.id]
                      : `${units}/${stop.quantity} un.`}
                  </span>
                </li>
              ))}
            </ul>
          )}
        </Card>
      )}

      {/* Rodapé fixo */}
      <div className="sticky bottom-0 -mx-4 mt-auto border-t bg-white px-4 pb-4 pt-3 md:-mx-8 md:px-8">
        {finished ? (
          <Button
            asChild
            className="h-12 w-full rounded-xl bg-orange text-base font-semibold text-white hover:bg-orange/90"
          >
            <Link href="/">Voltar para pedidos</Link>
          </Button>
        ) : (
          <>
            <p className="mb-2 text-center text-xs text-neutral-500">
              Confira o endereço antes de ler o código.
            </p>
            <Button
              type="button"
              onClick={handleScan}
              className="h-12 w-full rounded-xl bg-orange text-base font-semibold text-white hover:bg-orange/90"
            >
              <ScanLine className="size-5" />
              Ler código
            </Button>
          </>
        )}
      </div>
    </main>
  );
}