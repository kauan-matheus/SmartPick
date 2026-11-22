"use client";

import { useEffect, useState, useSyncExternalStore, type FormEvent } from "react";
import { useRouter } from "next/navigation";
import {
  ArrowRight,
  CircleCheck,
  CircleX,
  Eye,
  EyeOff,
  HelpCircle,
  LockKeyhole,
  LoaderCircle,
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
  const [loginStatus, setLoginStatus] = useState<
    "idle" | "loading" | "success" | "error"
  >("idle");
  const [error, setError] = useState("");
  const [helpMessage, setHelpMessage] = useState("");
  const [viewportScale, setViewportScale] = useState(1);
  const visualStatus =
    isAuthenticated && loginStatus === "idle" ? "loading" : loginStatus;

  useEffect(() => {
    function updateViewportScale() {
      const widthScale = window.innerWidth / 390;
      const heightScale = (window.innerHeight - 48) / 560;
      setViewportScale(
        Math.min(1.1, Math.max(0.75, Math.min(widthScale, heightScale))),
      );
    }

    updateViewportScale();
    window.addEventListener("resize", updateViewportScale);
    return () => window.removeEventListener("resize", updateViewportScale);
  }, []);

  useEffect(() => {
    if (getStoredAuth()) {
      router.replace("/tarefas-designadas");
    }
  }, [router]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setLoginStatus("loading");

    try {
      const auth = await login(username, password);
      sessionStorage.setItem(authStorageKey, JSON.stringify(auth));
      setLoginStatus("success");
      await new Promise((resolve) => setTimeout(resolve, 700));
      router.replace("/tarefas-designadas");
    } catch (caughtError) {
      setLoginStatus("error");
      setError(
        caughtError instanceof Error
          ? caughtError.message
          : "Não foi possível entrar. Tente novamente.",
      );
    }
  }

  return (
    <div className="relative isolate min-h-screen overflow-hidden bg-gradient-to-br from-orange-200 via-white to-orange-50">
      <header className="relative z-10 flex h-12 items-center bg-black px-4 text-white">
        <div className="flex items-center gap-2 font-semibold">
          <Warehouse className="size-4" aria-hidden="true" />
          SmartPick
        </div>
      </header>

      <div
        aria-hidden="true"
        className="pointer-events-none absolute inset-0 z-0 overflow-hidden"
      >
        <span className="absolute -left-24 top-[15%] size-72 rounded-full bg-orange-300/30 blur-3xl motion-safe:animate-pulse" />
        <span className="absolute -right-28 top-[40%] size-80 rounded-full bg-amber-300/25 blur-3xl motion-safe:animate-pulse" />
        <span className="absolute bottom-[-8rem] left-1/3 size-96 rounded-full bg-orange-200/40 blur-3xl motion-safe:animate-pulse" />
      </div>

      <main className="relative z-10 flex min-h-[calc(100svh-3rem)] w-full items-center justify-center px-4 py-8">
        <div
          style={{ zoom: viewportScale }}
          className="mx-auto flex w-full max-w-md flex-col"
        >
          <div
            aria-live="polite"
            className={`mb-1 flex h-11 w-full items-center justify-center gap-2 rounded-xl ${
              visualStatus === "success"
                ? "bg-emerald-100 text-emerald-700"
                : visualStatus === "error"
                  ? "bg-red-100 text-red-700"
                  : "bg-orange-soft text-orange"
            }`}
          >
            <span className="relative -top-0.5 flex">
              {visualStatus === "loading" ? (
                <LoaderCircle className="size-5 animate-spin" aria-hidden="true" />
              ) : visualStatus === "success" ? (
                <CircleCheck className="size-5" aria-hidden="true" />
              ) : visualStatus === "error" ? (
                <CircleX className="size-5" aria-hidden="true" />
              ) : (
                <LockKeyhole className="size-5" aria-hidden="true" />
              )}
            </span>
            {visualStatus !== "idle" && (
              <span className="sr-only">
                {visualStatus === "loading"
                  ? "Validando acesso"
                  : visualStatus === "success"
                    ? "Acesso validado"
                    : "Falha na validação do acesso"}
              </span>
            )}
            <span className="text-lg leading-none font-bold">Área de Acesso</span>
          </div>

        <div className="mb-4 rounded-xl bg-orange-soft p-3">
          <h1 className="text-xl font-bold text-neutral-900">
            Acesse seu trabalho
          </h1>
          <p className="mt-1 text-sm text-neutral-500">
            Entre para consultar seus pedidos e iniciar as tarefas de coleta.
          </p>
        </div>

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
            disabled={
              visualStatus === "loading" || visualStatus === "success"
            }
            className="mt-4 flex h-11 w-full items-center justify-center gap-2 rounded-xl bg-orange text-sm font-semibold text-white transition hover:bg-orange/90 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-orange disabled:cursor-wait disabled:opacity-60"
          >
            {visualStatus === "loading" ? "Entrando..." : "Entrar"}
            {visualStatus === "idle" || visualStatus === "error" ? (
              <ArrowRight className="size-4" aria-hidden="true" />
            ) : null}
          </button>
        </form>

        <section
          aria-labelledby="access-help-title"
          className="mt-3 rounded-xl bg-white p-3 "
        >
          <div className="flex items-center gap-2">
            <HelpCircle
              className="size-4 shrink-0 text-emerald-800"
              aria-hidden="true"
            />
            <span aria-hidden="true" className="w-px self-stretch bg-black" />
            <div className="min-w-0">
              <h2
                id="access-help-title"
                className="text-[clamp(0.625rem,3vw,0.75rem)] font-semibold text-neutral-800"
              >
                Primeiro acesso ou dificuldade para entrar?
              </h2>
              <p className="mt-1 text-[clamp(0.625rem,3vw,0.75rem)] leading-relaxed text-neutral-700">
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
        </div>
      </main>
    </div>
  );
}
