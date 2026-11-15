"use client";

import { useRouter } from "next/navigation";
import { LogOut, Warehouse } from "lucide-react";
import { clearStoredAuth } from "@/lib/auth-session";

export function AppHeader() {
  const router = useRouter();

  function handleLogout() {
    clearStoredAuth();
    router.replace("/login");
  }

  return (
    <header className="sticky top-0 z-10 flex h-12 items-center justify-between bg-black px-4 text-white md:px-8">
      <div className="flex items-center gap-2 font-semibold md:hidden">
        <Warehouse className="size-4" />
        SmartPick
      </div>
      <button
        type="button"
        onClick={handleLogout}
        className="ml-auto flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm text-white/90 transition hover:bg-white/10 hover:text-white focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-orange"
      >
        <LogOut className="size-4" aria-hidden="true" />
        Sair
      </button>
    </header>
  );
}