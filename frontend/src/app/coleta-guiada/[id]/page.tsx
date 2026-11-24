import { AppHeader } from "@/components/app-header";
import { AuthGuard } from "@/components/auth-guard";
import { OrderRouteContent } from "@/components/order-route-content";
import { Sidebar } from "@/components/sidebar";

export default function GuidedPickingPage() {
  return (
    <AuthGuard>
      <div className="min-h-screen bg-neutral-100 md:flex">
        <Sidebar />
        <div className="flex min-h-screen min-w-0 flex-1 flex-col">
          <AppHeader />
          <OrderRouteContent screen="picking" />
        </div>
      </div>
    </AuthGuard>
  );
}
