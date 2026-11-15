"use client";

import {
  useEffect,
  useSyncExternalStore,
  type ReactNode,
} from "react";
import { useRouter } from "next/navigation";
import { getStoredAuth } from "@/lib/auth-session";

function subscribeToStorage(callback: () => void) {
  window.addEventListener("storage", callback);
  return () => window.removeEventListener("storage", callback);
}

function hasStoredAuth() {
  return getStoredAuth() !== null;
}

export function AuthGuard({ children }: { children: ReactNode }) {
  const router = useRouter();
  const isAuthenticated = useSyncExternalStore(
    subscribeToStorage,
    hasStoredAuth,
    () => false,
  );

  useEffect(() => {
    if (!isAuthenticated) {
      router.replace("/login");
    }
  }, [isAuthenticated, router]);

  if (!isAuthenticated) {
    return (
      <main
        aria-live="polite"
        className="flex min-h-screen items-center justify-center bg-neutral-100 text-sm text-neutral-500"
      >
        Verificando acesso...
      </main>
    );
  }

  return children;
}
