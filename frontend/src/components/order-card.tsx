import { ArrowRight } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Card } from "@/components/ui/card";
import type { Order, OrderStatus } from "@/lib/orders";

const statusConfig: Record<
  OrderStatus,
  { label: string; badge: string; action: string }
> = {
  alerta: {
    label: "Alerta de prazo",
    badge: "bg-warn-bg text-warn-fg",
    action: "Ver tarefas",
  },
  aguardando: {
    label: "Aguardando",
    badge: "bg-muted-bg text-neutral-600",
    action: "Ver pedido",
  },
  concluido: {
    label: "Concluído",
    badge: "bg-orange-soft text-orange",
    action: "Ver pedido",
  },
};

export function OrderCard({ order }: { order: Order }) {
  const status = statusConfig[order.status];

  return (
    <Card className="h-full gap-0 rounded-2xl p-4 shadow-sm">
      <div className="flex items-center justify-between gap-2">
        <span className="text-xs tracking-wide text-neutral-500">
          PEDIDO #{order.id}
        </span>
        <Badge variant="secondary" className={status.badge}>
          {status.label}
        </Badge>
      </div>

      <h2 className="mt-2 text-lg font-semibold">{order.category}</h2>

      <dl className="mt-3 space-y-1.5 text-sm">
        <div className="flex justify-between gap-3">
          <dt className="shrink-0 text-neutral-500">Destinatário</dt>
          <dd className="text-right font-medium">{order.recipient}</dd>
        </div>
        <div className="flex justify-between gap-3">
          <dt className="shrink-0 text-neutral-500">Transportadora</dt>
          <dd className="text-right font-medium">
            {order.carrier} · Coleta {order.pickup}
          </dd>
        </div>
      </dl>

      <div className="mt-auto flex items-center justify-between border-t pt-3 text-sm">
        <span className="font-semibold">{order.units} unidades</span>
        <button className="flex items-center gap-1 font-semibold text-orange">
          {status.action}
          <ArrowRight className="size-4" />
        </button>
      </div>
    </Card>
  );
}