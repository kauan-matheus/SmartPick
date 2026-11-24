import { AppHeader } from "@/components/app-header";
import { AuthGuard } from "@/components/auth-guard";
import { OrderRouteContent } from "@/components/order-route-content";
import { Sidebar } from "@/components/sidebar";

export default function CollectionReviewPage() {
  return (
    <AuthGuard>
      <div className="min-h-screen bg-[#faf8f5] md:flex">
        <Sidebar />
        <div className="flex min-h-screen min-w-0 flex-1 flex-col">
          <AppHeader />
          <OrderRouteContent screen="review" />
        </div>
      </div>
    </AuthGuard>
  );
}
