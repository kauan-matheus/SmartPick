import { notFound } from "next/navigation";
import { AppHeader } from "@/components/app-header";
import { AuthGuard } from "@/components/auth-guard";
import { CollectionReview } from "@/components/collection-review";
import { Sidebar } from "@/components/sidebar";
import { orders } from "@/lib/orders";

interface CollectionReviewPageProps {
  params: Promise<{ id: string }>;
  searchParams: Promise<{ tipo?: string }>;
}

export default async function CollectionReviewPage({
  params,
  searchParams,
}: CollectionReviewPageProps) {
  const { id } = await params;
  const { tipo } = await searchParams;
  const order = orders.find((item) => item.id === Number(id));

  if (!order || (tipo !== "light" && tipo !== "heavy")) {
    notFound();
  }

  return (
    <AuthGuard>
      <div className="min-h-screen bg-neutral-100 md:flex">
        <Sidebar />

        <div className="flex min-h-screen min-w-0 flex-1 flex-col">
          <AppHeader />
          <CollectionReview order={order} taskType={tipo} />
        </div>
      </div>
    </AuthGuard>
  );
}
