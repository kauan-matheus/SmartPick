import { AppHeader } from "@/components/app-header";
import { AuthGuard } from "@/components/auth-guard";
import { BottomNav } from "@/components/bottom-nav";
import { ProfileScreen } from "@/components/profile-screen";
import { Sidebar } from "@/components/sidebar";

export default function MyDataPage() {
  return (
    <AuthGuard>
      <div className="min-h-screen bg-neutral-100 md:flex">
        <Sidebar />
        <div className="flex min-h-screen min-w-0 flex-1 flex-col">
          <AppHeader />
          <ProfileScreen />
        </div>
        <BottomNav />
      </div>
    </AuthGuard>
  );
}
