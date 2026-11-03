import { AppHeader } from "@/components/app-header";
import { BottomNav } from "@/components/bottom-nav";
import { MyTasksScreen } from "@/components/my-tasks-screen";
import { Sidebar } from "@/components/sidebar";

export default function MyTasksPage() {
  return (
    <div className="min-h-screen bg-neutral-100 md:flex">
      <Sidebar />

      <div className="flex min-h-screen min-w-0 flex-1 flex-col">
        <AppHeader />
        <MyTasksScreen />
      </div>

      <BottomNav />
    </div>
  );
}
