import { notFound } from "next/navigation";
import { AppHeader } from "@/components/app-header";
import { AssignedTaskDetails } from "@/components/assigned-task-details";
import { Sidebar } from "@/components/sidebar";
import { orders } from "@/lib/orders";

interface AssignedTaskPageProps {
  params: Promise<{ id: string }>;
}

export default async function AssignedTaskPage({
  params,
}: AssignedTaskPageProps) {
  const { id } = await params;
  const order = orders.find((item) => item.id === Number(id));

  if (!order) {
    notFound();
  }

  return (
    <div className="min-h-screen bg-neutral-100 md:flex">
      <Sidebar />

      <div className="flex min-h-screen min-w-0 flex-1 flex-col">
        <AppHeader />
        <AssignedTaskDetails order={order} />
      </div>
    </div>
  );
}
