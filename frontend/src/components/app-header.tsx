"use client";

import { useRouter } from "next/navigation";
import { LogOut, Warehouse } from "lucide-react";
import { clearSession } from "@/lib/auth-session";

export function AppHeader() {
  const router = useRouter();

  function handleLogout() {
    clearSession();
    router.replace("/login");
  }

  return (
    <header className="sticky top-0 z-10 flex h-14 items-center justify-between border-b border-[#e8e2d9] bg-[#faf8f5]/95 px-4 text-[#1c1917] backdrop-blur md:px-8">
      <div className="flex items-center gap-2 font-semibold md:hidden">
        <span className="flex size-8 items-center justify-center rounded-lg bg-orange-soft text-orange">
          <Warehouse className="size-4" />
        </span>
        SmartPick
      </div>
      <button
        type="button"
        onClick={handleLogout}
        className="ml-auto flex min-h-10 items-center gap-2 rounded-xl px-3 py-1.5 text-sm text-neutral-700 transition hover:bg-orange-soft hover:text-orange focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-orange"
      >
        <LogOut className="size-4" aria-hidden="true" />
        Sair
      </button>
    </header>
  );
}
