"use client";

import { useState, useSyncExternalStore } from "react";
import { useRouter } from "next/navigation";
import {
  ArrowLeft,
  ArrowRight,
  Check,
  CircleAlert,
  CircleCheck,
  Forklift,
  MapPin,
  Package,
  PackageOpen,
  TriangleAlert,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import {
  getCollectionReviewStorageKey,
  subscribeToCollectionReview,
  type CollectionReviewData,
} from "@/lib/collection-session";
import type { PickingPlan, PickingStop, TaskType } from "@/lib/picking";
import type { Order } from "@/lib/orders";
import { getErrorMessage } from "@/lib/auth";
import { finishTasks } from "@/lib/tasks";
import { cn } from "@/lib/utils";

interface CollectionReviewProps {
  order: Order;
  taskType: TaskType;
  stopsByType: PickingPlan;
}

const taskTypes: TaskType[] = ["light", "heavy"];

function getServerSnapshot() {
  return "";
}

function parseReviewData(
  snapshot: string,
  stops: PickingStop[]
): CollectionReviewData | null {
  if (!snapshot) return null;

  try {
    const parsed: unknown = JSON.parse(snapshot);
    if (
      typeof parsed !== "object" ||
      parsed === null ||
      !("collected" in parsed) ||
      !Array.isArray(parsed.collected) ||
      parsed.collected.length !== stops.length ||
      !("justified" in parsed) ||
      typeof parsed.justified !== "object" ||
      parsed.justified === null ||
      Array.isArray(parsed.justified)
    ) {
      return null;
    }

    const collectedValues = parsed.collected;
    const justifiedValues = parsed.justified;
    const collected: number[] = [];
    for (const [index, stop] of stops.entries()) {
      const quantity = collectedValues[index];
      if (
        typeof quantity !== "number" ||
        !Number.isInteger(quantity) ||
        quantity < 0 ||
        quantity > stop.quantity
      ) {
        return null;
      }
      collected.push(quantity);
    }

    const justified: Record<string, string> = {};
    for (const [stopId, reason] of Object.entries(justifiedValues)) {
      if (
        !stops.some((stop) => stop.id === stopId) ||
        typeof reason !== "string" ||
        !reason.trim()
      ) {
        return null;
      }
      justified[stopId] = reason;
    }

    const data = { collected, justified };
    return stops.every(
      (stop, index) =>
        collected[index] >= stop.quantity || Boolean(justified[stop.id])
    )
      ? data
      : null;
  } catch {
    return null;
  }
}

export function CollectionReview({ order, taskType, stopsByType }: CollectionReviewProps) {
  const router = useRouter();
  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState("");
  const stops = stopsByType[taskType];
  const storageKey = getCollectionReviewStorageKey(order.id, taskType);
  const otherTaskType = taskType === "light" ? "heavy" : "light";
  const otherStorageKey = getCollectionReviewStorageKey(
    order.id,
    otherTaskType
  );
  const emptyReviewData: CollectionReviewData = {
    collected: stops.map(() => 0),
    justified: {},
  };
  const reviewSnapshot = useSyncExternalStore(
    subscribeToCollectionReview,
    () => sessionStorage.getItem(storageKey) ?? "",
    getServerSnapshot
  );
  const completedTypesSnapshot = useSyncExternalStore(
    subscribeToCollectionReview,
    () =>
      taskTypes
        .filter((type) => {
          const taskStops = stopsByType[type];
          const taskSnapshot = sessionStorage.getItem(
            getCollectionReviewStorageKey(order.id, type)
          );
          return (
            taskStops.length === 0 ||
            parseReviewData(taskSnapshot ?? "", taskStops) !== null
          );
        })
        .join(","),
    getServerSnapshot
  );
  const reviewData =
    parseReviewData(reviewSnapshot, stops) ?? emptyReviewData;
  const completedTaskTypes = completedTypesSnapshot
    ? (completedTypesSnapshot.split(",") as TaskType[])
    : [];
  const canConclude =
    completedTaskTypes.includes(taskType) &&
    completedTaskTypes.includes(otherTaskType);
  const totalUnits = stops.reduce((sum, stop) => sum + stop.quantity, 0);
  const collectedUnits = reviewData.collected.reduce(
    (sum, quantity) => sum + quantity,
    0
  );
  const missingUnits = stops.reduce(
    (sum, stop, index) =>
      sum +
      (reviewData.justified[stop.id]
        ? Math.max(0, stop.quantity - reviewData.collected[index])
        : 0),
    0
  );
  const validatedAddresses = stops.filter(
    (stop, index) =>
      reviewData.collected[index] >= stop.quantity ||
      Boolean(reviewData.justified[stop.id])
  ).length;
  const taskCode = `${taskType === "heavy" ? "P" : "L"}07`;

  async function handleConclude() {
    if (!canConclude || submitting) return;
    setSubmitting(true);
    setSubmitError("");

    try {
      const taskIds = [...stopsByType.light, ...stopsByType.heavy].map(
        (stop) => stop.id,
      );
      await finishTasks(taskIds);
      sessionStorage.removeItem(storageKey);
      sessionStorage.removeItem(otherStorageKey);
      router.push("/tarefas-designadas");
    } catch (error) {
      setSubmitError(
        getErrorMessage(error, "Não foi possível registrar a conclusão das tarefas."),
      );
      setSubmitting(false);
    }
  }

  return (
    <main className="flex w-full flex-1 flex-col px-4 pt-5 md:px-8 md:pt-8">
      <div className="-mx-4 -mt-5 border-b border-[#e8e2d9] bg-[#faf8f5] px-4 py-4 md:-mx-8 md:-mt-8 md:px-8 md:py-6">
        <div className="flex items-center gap-3">
          <button
            type="button"
            onClick={() => router.push(`/tarefas-designadas/${order.id}`)}
            aria-label="Voltar para tarefas designadas"
            className="flex size-10 shrink-0 items-center justify-center rounded-xl border border-[#e8e2d9] bg-white text-neutral-700 transition hover:bg-[#f3efe9]"
          >
            <ArrowLeft className="size-5" />
          </button>
          <div>
            <h1 className="text-xl font-semibold md:text-2xl">Revisar coleta</h1>
            <p className="mt-1 text-sm text-neutral-500">
              Histórico coletado · Tarefa {taskCode}
            </p>
          </div>
        </div>
      </div>

      <section
        className="mt-4 grid grid-cols-1 gap-3 sm:grid-cols-2"
        aria-label="Resumo da revisão"
      >
        <Card className="relative items-center justify-center rounded-2xl p-3 text-center shadow-sm">
          <span
            aria-hidden="true"
            className="absolute inset-y-0 left-0 flex items-center pl-4 text-neutral-800 sm:pl-6 md:pl-8"
          >
            <Package className="size-6" strokeWidth={1.8} />
          </span>
          <div className="flex w-full flex-col items-center gap-0.5 text-center">
            <p className="text-2xl leading-none font-bold text-orange">
              {collectedUnits} de {totalUnits}
            </p>
            <p className="text-center text-xs leading-none text-neutral-500">
              unidades coletadas
            </p>
          </div>
        </Card>
        <Card className="relative items-center justify-center rounded-2xl p-3 text-center shadow-sm">
          <span
            aria-hidden="true"
            className="absolute inset-y-0 left-0 flex items-center pl-4 text-neutral-800 sm:pl-6 md:pl-8"
          >
            {taskType === "heavy" ? (
              <Forklift className="size-6" strokeWidth={1.8} />
            ) : (
              <PackageOpen className="size-6" strokeWidth={1.8} />
            )}
          </span>
          <div className="flex w-full flex-col items-center gap-0.5 text-center">
            <p className="text-2xl leading-none font-bold text-orange">
              {validatedAddresses} de {stops.length}
            </p>
            <p className="text-center text-xs leading-none text-neutral-500">
              endereços validados
            </p>
          </div>
        </Card>
      </section>

      {taskType === "heavy" && (
        <div
          role="note"
          className="mt-4 flex items-center gap-3 rounded-xl border border-orange/30 bg-orange-soft p-3 text-sm text-warn-fg"
        >
          <CircleAlert className="size-5 shrink-0 text-orange" />
          <p>
            Lembre-se que após o uso é necessário guardar o maquinário.
          </p>
        </div>
      )}

      {missingUnits > 0 && (
        <div
          role="alert"
          className="mt-4 flex gap-3 rounded-xl border border-orange/30 bg-warn-bg p-3"
        >
          <TriangleAlert className="mt-0.5 size-5 shrink-0 text-orange" />
          <div className="text-sm">
            <p className="font-semibold text-warn-fg">
              {missingUnits}{" "}
              {missingUnits === 1
                ? "unidade não coletada"
                : "unidades não coletadas"}
            </p>
            <p className="mt-0.5 text-xs text-warn-fg">
              A tarefa pode ser concluída com ressalvas.
            </p>
          </div>
        </div>
      )}

      <section className="mt-4" aria-labelledby="review-items-title">
        <div className="mb-2 flex items-center justify-between">
          <h2 id="review-items-title" className="font-semibold">
            Itens coletados
          </h2>
          <span className="text-xs text-neutral-500">Checado / status</span>
        </div>

        <Card className="gap-0 divide-y rounded-2xl shadow-sm">
          {stops.map((stop, index) => {
            const collected = reviewData.collected[index] ?? 0;
            const justified = reviewData.justified[stop.id];
            const complete = collected >= stop.quantity;

            return (
              <div
                key={stop.id}
                className="grid grid-cols-[3rem_minmax(0,1fr)_3rem] items-center px-4 py-3"
              >
                <span className="text-lg font-bold">{stop.address}</span>
                <div className="min-w-0">
                  <p className="font-semibold">{stop.name}</p>
                  <p className="mt-0.5 text-xs text-neutral-500">
                    {stop.detail}
                  </p>
                  {justified && (
                    <p className="mt-0.5 text-xs text-warn-fg">
                      Justificativa: {justified}
                    </p>
                  )}
                </div>
                <div className="flex items-end justify-center">
                  <div className="flex flex-col items-end">
                    <span
                      className={cn(
                        "text-sm font-semibold",
                        complete ? "text-foreground" : "text-orange"
                      )}
                    >
                      {collected}/{stop.quantity}
                    </span>
                    {complete ? (
                      <CircleCheck className="mt-1 size-5 text-emerald-600" />
                    ) : justified ? (
                      <TriangleAlert className="mt-1 size-5 text-orange" />
                    ) : (
                      <CircleAlert className="mt-1 size-5 text-neutral-400" />
                    )}
                  </div>
                </div>
              </div>
            );
          })}
        </Card>
      </section>

      <div className="sticky bottom-0 -mx-4 mt-auto border-t border-[#e8e2d9] bg-[#faf8f5]/95 px-4 pb-4 pt-3 backdrop-blur md:-mx-8 md:px-8">
        {!completedTaskTypes.includes(otherTaskType) &&
          stopsByType[otherTaskType].length > 0 && (
          <Button
            type="button"
            variant="outline"
            onClick={() =>
              router.push(
                `/coleta-guiada/${order.id}?tipo=${otherTaskType}`
              )
            }
            className="mb-3 h-12 w-full rounded-xl border border-orange/30 bg-orange-soft text-base font-semibold text-orange hover:bg-orange/15 hover:text-orange"
          >
            <ArrowRight className="size-5" />
            Coleta {otherTaskType === "heavy" ? "Pesada" : "Leve"}
          </Button>
        )}
        <p className="mb-2 flex items-center justify-center gap-2 text-xs text-neutral-500">
          <MapPin className="size-4 text-orange" />
          <span>
            Leve os {collectedUnits} volumes à área de expedição E2
          </span>
        </p>
        <p className="mb-3 text-center text-[11px] text-neutral-500">
          Confira os volumes antes de concluir.
        </p>
        {submitError && (
          <p role="alert" className="mb-3 text-center text-sm text-red-700">
            {submitError}
          </p>
        )}
        <Button
          type="button"
          onClick={handleConclude}
          disabled={!canConclude || submitting}
          className="h-12 w-full rounded-xl bg-orange text-base font-semibold text-white hover:bg-orange/90"
        >
          <Check className="size-5" />
          {submitting ? "Registrando tarefas..." : "Concluir tarefa"}
        </Button>
      </div>
    </main>
  );
}
