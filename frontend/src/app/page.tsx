"use client";

import { useEffect, useSyncExternalStore } from "react";
import { useRouter } from "next/navigation";
import { getStoredAuth } from "@/lib/auth-session";

function subscribeToStorage(callback: () => void) {
  window.addEventListener("storage", callback);
  return () => window.removeEventListener("storage", callback);
}

function hasStoredAuth() {
  return getStoredAuth() !== null;
}

export default function Home() {
  const router = useRouter();
  const isAuthenticated = useSyncExternalStore(
    subscribeToStorage,
    hasStoredAuth,
    () => false,
  );

  useEffect(() => {
    router.replace(
      isAuthenticated ? "/tarefas-designadas" : "/login",
    );
  }, [isAuthenticated, router]);

  return (
    <main
      aria-live="polite"
      className="flex min-h-screen items-center justify-center bg-[#faf8f5] text-sm text-neutral-500"
    >
      Verificando acesso...
    </main>
  );
}
