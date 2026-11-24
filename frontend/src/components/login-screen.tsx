"use client";

import { useEffect, useState, type FormEvent } from "react";
import { useRouter } from "next/navigation";
import {
  ArrowRight,
  CircleCheck,
  Eye,
  EyeOff,
  HelpCircle,
  LoaderCircle,
  LockKeyhole,
  Mail,
  Warehouse,
} from "lucide-react";
import Aurora from "@/components/Aurora";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { login } from "@/lib/auth";
import { authStorageKey, getStoredAuth } from "@/lib/auth-session";

export function LoginScreen() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [loginStatus, setLoginStatus] = useState<"idle" | "loading" | "success" | "error">("idle");
  const [error, setError] = useState("");
  const [helpMessage, setHelpMessage] = useState("");

  useEffect(() => {
    if (getStoredAuth()) router.replace("/tarefas-designadas");
  }, [router]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setLoginStatus("loading");

    try {
      const auth = await login(email, password);
      sessionStorage.setItem(authStorageKey, JSON.stringify(auth));
      setLoginStatus("success");
      await new Promise((resolve) => setTimeout(resolve, 450));
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
    <div className="relative isolate min-h-dvh overflow-hidden bg-[#faf8f5] text-neutral-950">
      <div aria-hidden="true" className="pointer-events-none absolute inset-x-0 top-0 h-[25rem] opacity-35">
        <Aurora
          colorStops={["#f6b56e", "#f8d9b7", "#fff0df"]}
          amplitude={0.8}
          blend={0.65}
          speed={0.3}
          height={0.55}
          layers={1}
          mouseInteraction={false}
          lightMode
          opacity={0.5}
          className="h-full w-full"
        />
      </div>

      <header className="relative z-10 mx-auto flex h-16 w-full max-w-lg items-center px-5">
        <div className="flex items-center gap-2.5">
          <span className="flex size-9 items-center justify-center rounded-xl bg-neutral-950 text-white">
            <Warehouse className="size-[18px]" aria-hidden="true" />
          </span>
          <span className="text-base font-semibold">SmartPick</span>
        </div>
      </header>

      <main className="relative z-10 mx-auto flex min-h-[calc(100dvh-4rem)] w-full max-w-lg items-start justify-center px-5 pb-[max(2rem,env(safe-area-inset-bottom))] pt-8 sm:items-center sm:pt-0">
        <div className="login-enter w-full max-w-[25rem]">
          <div className="mb-6">
            <p className="mb-2 text-sm font-medium text-orange">Acesso à operação</p>
            <h1 className="text-[1.8rem] leading-tight font-semibold tracking-normal">
              Bem-vindo de volta
            </h1>
            <p className="mt-2 text-sm leading-6 text-neutral-600">
              Entre na sua conta para continuar.
            </p>
          </div>

          <Card className="gap-0 rounded-2xl border border-neutral-200/80 bg-white p-5 shadow-[0_12px_36px_-28px_rgba(40,28,12,0.35)] sm:p-6">
            <form onSubmit={handleSubmit} className="space-y-5">
              <div className="space-y-2">
                <label htmlFor="email" className="block text-sm font-medium text-neutral-800">
                  E-mail
                </label>
                <div className="relative">
                  <Mail className="pointer-events-none absolute left-3.5 top-1/2 size-[18px] -translate-y-1/2 text-neutral-400" aria-hidden="true" />
                  <Input
                    id="email"
                    name="email"
                    type="email"
                    inputMode="email"
                    autoComplete="email"
                    autoCapitalize="none"
                    spellCheck={false}
                    required
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    placeholder="voce@empresa.com"
                    aria-invalid={Boolean(error)}
                    className="h-12 rounded-xl border-neutral-200 bg-white pl-11 text-base placeholder:text-neutral-400 focus-visible:border-orange focus-visible:ring-orange/20"
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex items-center justify-between gap-3">
                  <label htmlFor="password" className="text-sm font-medium text-neutral-800">
                    Senha
                  </label>
                  <button
                    type="button"
                    onClick={() =>
                      setHelpMessage(
                        "A recuperação de senha ainda não está disponível. Procure o líder do seu turno.",
                      )
                    }
                    className="min-h-11 px-1 text-sm font-medium text-orange underline-offset-4 hover:underline focus-visible:rounded focus-visible:outline-2 focus-visible:outline-orange"
                  >
                    Esqueci minha senha
                  </button>
                </div>
                <div className="relative">
                  <LockKeyhole className="pointer-events-none absolute left-3.5 top-1/2 size-[18px] -translate-y-1/2 text-neutral-400" aria-hidden="true" />
                  <Input
                    id="password"
                    name="password"
                    type={passwordVisible ? "text" : "password"}
                    autoComplete="current-password"
                    required
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    placeholder="Digite sua senha"
                    aria-invalid={Boolean(error)}
                    className="h-12 rounded-xl border-neutral-200 bg-white px-11 text-base placeholder:text-neutral-400 focus-visible:border-orange focus-visible:ring-orange/20"
                  />
                  <button
                    type="button"
                    onClick={() => setPasswordVisible((visible) => !visible)}
                    aria-label={passwordVisible ? "Ocultar senha" : "Mostrar senha"}
                    aria-pressed={passwordVisible}
                    className="absolute inset-y-0 right-0 flex min-h-11 w-11 items-center justify-center rounded-r-xl text-neutral-500 hover:text-neutral-900 focus-visible:outline-2 focus-visible:outline-orange"
                  >
                    {passwordVisible ? (
                      <EyeOff className="size-[18px]" aria-hidden="true" />
                    ) : (
                      <Eye className="size-[18px]" aria-hidden="true" />
                    )}
                  </button>
                </div>
              </div>

              {error && (
                <p role="alert" className="rounded-xl border border-red-200 bg-red-50 px-3.5 py-3 text-sm leading-5 text-red-700">
                  {error}
                </p>
              )}
              {helpMessage && (
                <p role="status" className="rounded-xl bg-orange-50 px-3.5 py-3 text-sm leading-5 text-neutral-700">
                  {helpMessage}
                </p>
              )}

              <Button
                type="submit"
                disabled={loginStatus === "loading" || loginStatus === "success"}
                className="h-12 w-full rounded-xl bg-orange text-base font-semibold text-white shadow-sm transition-colors hover:bg-orange/90 focus-visible:ring-orange/30 disabled:cursor-wait"
              >
                {loginStatus === "loading" ? (
                  <>
                    <LoaderCircle className="size-5 animate-spin" aria-hidden="true" />
                    Entrando...
                  </>
                ) : loginStatus === "success" ? (
                  <>
                    <CircleCheck className="size-5" aria-hidden="true" />
                    Acesso validado
                  </>
                ) : (
                  <>
                    Entrar
                    <ArrowRight className="size-5" aria-hidden="true" />
                  </>
                )}
              </Button>
            </form>
          </Card>

          <div className="mt-5 flex items-start gap-2.5 px-1 text-xs leading-5 text-neutral-500">
            <HelpCircle className="mt-0.5 size-4 shrink-0" aria-hidden="true" />
            <p>Acesso exclusivo para funcionários. Não compartilhe sua senha.</p>
          </div>
        </div>
      </main>
    </div>
  );
}
