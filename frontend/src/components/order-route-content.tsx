"use client";

import { useEffect, useState } from "react";
import { useParams, useSearchParams } from "next/navigation";
import { AssignedTaskDetails } from "@/components/assigned-task-details";
import { CollectionReview } from "@/components/collection-review";
import { GuidedPicking } from "@/components/guided-picking";
import { getErrorMessage } from "@/lib/auth";
import { getOrderById, type Order } from "@/lib/orders";
import type { TaskType } from "@/lib/picking";

type Screen = "details" | "picking" | "review";

export function OrderRouteContent({ screen }: { screen: Screen }) {
  const params = useParams<{ id: string }>();
  const searchParams = useSearchParams();
  const taskTypeValue = searchParams.get("tipo");
  const taskType: TaskType | null =
    taskTypeValue === "light" || taskTypeValue === "heavy"
      ? taskTypeValue
      : null;
  const [order, setOrder] = useState<Order | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError("");

    getOrderById(params.id)
      .then((result) => {
        if (cancelled) return;
        if (!result) setError("Este pedido não está disponível.");
        else setOrder(result);
      })
      .catch((cause: unknown) => {
        if (!cancelled) {
          setError(getErrorMessage(cause, "Não foi possível carregar o pedido."));
        }
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
  }, [params.id]);

  if (loading) {
    return <p className="flex-1 px-4 py-12 text-center text-sm text-neutral-500">Carregando pedido...</p>;
  }
  if (error || !order) {
    return <p role="alert" className="flex-1 px-4 py-12 text-center text-sm text-red-700">{error || "Pedido não encontrado."}</p>;
  }
  if (screen !== "details" && !taskType) {
    return <p role="alert" className="flex-1 px-4 py-12 text-center text-sm text-red-700">Tipo de tarefa inválido.</p>;
  }

  if (screen === "details") return <AssignedTaskDetails order={order} />;
  if (screen === "picking") return <GuidedPicking order={order} taskType={taskType!} />;
  return <CollectionReview order={order} taskType={taskType!} />;
}
