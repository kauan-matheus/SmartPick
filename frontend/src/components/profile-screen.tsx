"use client";

import { useEffect, useState } from "react";
import { Mail, UserRound } from "lucide-react";
import { Card } from "@/components/ui/card";
import { getStoredAuth } from "@/lib/auth-session";
import type { AuthResponse } from "@/lib/auth";

type UserProfile = AuthResponse["usuario"];

function getRoleLabel(type: string) {
  if (type === "MANAGER") return "Gerente";
  if (type === "EMPLOYEE") return "Funcionário";
  return type;
}

export function ProfileScreen() {
  const [user, setUser] = useState<UserProfile | null>(null);

  useEffect(() => {
    setUser(getStoredAuth()?.usuario ?? null);
  }, []);

  return (
    <main className="mx-auto w-full max-w-3xl flex-1 px-4 pb-28 pt-5 md:px-8 md:pb-10 md:pt-8">
      <div>
        <h1 className="text-2xl font-bold md:text-3xl">Meus Dados</h1>
        <p className="mt-1 text-sm text-neutral-500">
          Informações da sua conta SmartPick.
        </p>
      </div>

      {!user ? (
        <p role="status" className="mt-8 text-center text-sm text-neutral-500">
          Carregando dados do usuário...
        </p>
      ) : (
        <Card className="mt-5 gap-0 rounded-2xl p-4 shadow-sm sm:p-6">
          <div className="flex items-center gap-3 border-b pb-4">
            <span className="flex size-12 items-center justify-center rounded-full bg-orange-soft text-orange">
              <UserRound className="size-6" />
            </span>
            <div className="min-w-0">
              <h2 className="break-words text-lg font-semibold">{user.nome}</h2>
              <span className="mt-1 block text-sm text-neutral-600">
                {getRoleLabel(user.type)}
              </span>
            </div>
          </div>

          <dl className="mt-4 space-y-4 text-sm">
            <div>
              <dt className="flex items-center gap-2 text-neutral-500">
                <Mail className="size-4" /> E-mail
              </dt>
              <dd className="mt-1 break-all font-medium">{user.email}</dd>
            </div>
            <div>
              <dt className="text-neutral-500">Identificador da conta</dt>
              <dd className="mt-1 break-all font-medium">{user.id}</dd>
            </div>
          </dl>
        </Card>
      )}
    </main>
  );
}
