import { AppHeader } from "@/components/app-header";
import { AuthGuard } from "@/components/auth-guard";
import { BottomNav } from "@/components/bottom-nav";
import { OrdersScreen } from "@/components/orders-screen";
import { Sidebar } from "@/components/sidebar";

export default function AssignedTasksPage() {
  return (
    <AuthGuard>
      <div className="min-h-screen bg-[#faf8f5] md:flex">
        <Sidebar />
        <div className="flex min-h-screen min-w-0 flex-1 flex-col">
          <AppHeader />
          <OrdersScreen />
        </div>
        <BottomNav />
      </div>
    </AuthGuard>
  );
}
