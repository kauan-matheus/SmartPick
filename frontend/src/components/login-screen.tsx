"use client";

import { useEffect, useState, useSyncExternalStore, type FormEvent } from "react";
import { useRouter } from "next/navigation";
import {
  ArrowRight,
  Eye,
  EyeOff,
  HelpCircle,
  LockKeyhole,
  Warehouse,
} from "lucide-react";
import { login } from "@/lib/auth";
import { authStorageKey, getStoredAuth } from "@/lib/auth-session";

function subscribeToStorage(callback: () => void) {
  window.addEventListener("storage", callback);
  return () => window.removeEventListener("storage", callback);
}

function hasStoredAuth() {
  return getStoredAuth() !== null;
}

export function LoginScreen() {
  const router = useRouter();
  const isAuthenticated = useSyncExternalStore(
    subscribeToStorage,
    hasStoredAuth,
    () => false,
  );
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");
  const [helpMessage, setHelpMessage] = useState("");

  useEffect(() => {
    if (getStoredAuth()) {
      router.replace("/tarefas-designadas");
    }
  }, [router]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setIsSubmitting(true);

    try {
      const auth = await login(username, password);
      sessionStorage.setItem(authStorageKey, JSON.stringify(auth));
      router.replace("/tarefas-designadas");
    } catch (caughtError) {
      setError(
        caughtError instanceof Error
          ? caughtError.message
          : "Não foi possível entrar. Tente novamente.",
      );
    } finally {
      setIsSubmitting(false);
    }
  }

  if (isAuthenticated) {
    return (
      <main
        aria-live="polite"
        className="flex min-h-screen items-center justify-center bg-neutral-100 text-sm text-neutral-500"
      >
        Verificando acesso...
      </main>
    );
  }

  return (
    <div className="min-h-screen bg-neutral-100">
      <header className="flex h-12 items-center bg-black px-4 text-white">
        <div className="flex items-center gap-2 font-semibold">
          <Warehouse className="size-4" aria-hidden="true" />
          SmartPick
        </div>
      </header>

      <main className="mx-auto flex w-full max-w-md flex-col px-4 pb-8 pt-8 sm:pt-12">
        <div className="mb-3 flex size-11 items-center justify-center rounded-xl bg-orange-soft text-orange">
          <LockKeyhole className="size-5" aria-hidden="true" />
        </div>

        <h1 className="text-xl font-bold text-neutral-900">
          Acesse seu trabalho
        </h1>
        <p className="mt-1 text-sm text-neutral-500">
          Entre para consultar seus pedidos e iniciar as tarefas de coleta.
        </p>

        <form
          onSubmit={handleSubmit}
          className="mt-4 rounded-2xl border border-neutral-200 bg-white p-3 sm:p-4"
        >
          <div className="space-y-1.5">
            <label
              htmlFor="username"
              className="block text-xs font-semibold text-neutral-800"
            >
              Usuário
            </label>
            <input
              id="username"
              name="username"
              type="text"
              autoComplete="username"
              required
              value={username}
              onChange={(event) => setUsername(event.target.value)}
              placeholder="Digite seu usuário"
              className="h-10 w-full rounded-lg border border-neutral-200 bg-white px-3 text-sm outline-none transition focus-visible:border-orange focus-visible:ring-2 focus-visible:ring-orange/20"
            />
          </div>

          <div className="mt-3 space-y-1.5">
            <label
              htmlFor="password"
              className="block text-xs font-semibold text-neutral-800"
            >
              Senha
            </label>
            <div className="relative">
              <input
                id="password"
                name="password"
                type={passwordVisible ? "text" : "password"}
                autoComplete="current-password"
                required
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Digite sua senha"
                className="h-10 w-full rounded-lg border border-neutral-200 bg-white px-3 pr-11 text-sm outline-none transition focus-visible:border-orange focus-visible:ring-2 focus-visible:ring-orange/20"
              />
              <button
                type="button"
                onClick={() => setPasswordVisible((visible) => !visible)}
                aria-label={passwordVisible ? "Ocultar senha" : "Mostrar senha"}
                aria-pressed={passwordVisible}
                className="absolute inset-y-0 right-0 flex w-10 items-center justify-center text-neutral-500 hover:text-neutral-800 focus-visible:outline-2 focus-visible:outline-offset-[-3px] focus-visible:outline-orange"
              >
                {passwordVisible ? (
                  <EyeOff className="size-4" aria-hidden="true" />
                ) : (
                  <Eye className="size-4" aria-hidden="true" />
                )}
              </button>
            </div>
          </div>

          <div className="mt-4 flex justify-end">
            <button
              type="button"
              onClick={() =>
                setHelpMessage(
                  "A recuperação de senha ainda não está disponível. Procure o líder do seu turno.",
                )
              }
              className="text-xs font-medium text-orange hover:underline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-orange"
            >
              Esqueci minha senha
            </button>
          </div>

          {error && (
            <p
              role="alert"
              className="mt-3 rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700"
            >
              {error}
            </p>
          )}

          <button
            type="submit"
            disabled={isSubmitting}
            className="mt-4 flex h-11 w-full items-center justify-center gap-2 rounded-xl bg-orange text-sm font-semibold text-white transition hover:bg-orange/90 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-orange disabled:cursor-wait disabled:opacity-60"
          >
            {isSubmitting ? "Entrando..." : "Entrar"}
            {!isSubmitting && (
              <ArrowRight className="size-4" aria-hidden="true" />
            )}
          </button>
        </form>

        <section
          aria-labelledby="access-help-title"
          className="mt-3 rounded-xl bg-muted-bg p-3"
        >
          <div className="flex items-start gap-2">
            <HelpCircle
              className="mt-0.5 size-4 shrink-0 text-emerald-800"
              aria-hidden="true"
            />
            <div>
              <h2
                id="access-help-title"
                className="text-xs font-semibold text-neutral-800"
              >
                Primeiro acesso ou dificuldade para entrar?
              </h2>
              <p className="mt-1 text-xs leading-relaxed text-neutral-500">
                Procure o líder do seu turno para obter ou liberar seu acesso.
              </p>
              {helpMessage && (
                <p role="status" className="mt-2 text-xs text-neutral-700">
                  {helpMessage}
                </p>
              )}
            </div>
          </div>
        </section>

        <p className="mt-4 text-center text-xs leading-relaxed text-neutral-500">
          Acesso exclusivo para funcionários.
          <br />
          Não compartilhe sua senha.
        </p>
      </main>
    </div>
  );
}
