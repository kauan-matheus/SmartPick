import { useSyncExternalStore } from "react";
import { orders } from "@/lib/orders";

const STORAGE_KEY = "smartpick:assigned-tasks";
const CHANGE_EVENT = "smartpick:assigned-tasks-changed";
const orderIds = new Set(orders.map((order) => order.id));

function parseTarefasDesignadas(saved: string | null): number[] {
  if (!saved) return [];

  let parsed: unknown;
  try {
    parsed = JSON.parse(saved);
  } catch (error) {
    console.error("Não foi possível ler as tarefas salvas.", error);
    return [];
  }

  if (!Array.isArray(parsed)) {
    console.error("A lista de tarefas salvas está em um formato inválido.");
    return [];
  }

  return [...new Set(parsed)].filter(
    (id): id is number => typeof id === "number" && orderIds.has(id)
  );
}

export function getTarefasDesignasID(
  saved: string | null = window.localStorage.getItem(STORAGE_KEY)
) {
  return parseTarefasDesignadas(saved);
}

function designar(onStoreChange: () => void) {
  window.addEventListener("storage", onStoreChange);
  window.addEventListener(CHANGE_EVENT, onStoreChange);
  return () => {
    window.removeEventListener("storage", onStoreChange);
    window.removeEventListener(CHANGE_EVENT, onStoreChange);
  };
}

function getSnapshot() {
  return window.localStorage.getItem(STORAGE_KEY);
}

export function useTarefasDesignasID() {
  const saved = useSyncExternalStore(designar, getSnapshot, () => null);
  return getTarefasDesignasID(saved);
}

export function saveTarefasDesignadasID(ids: number[]) {
  window.localStorage.setItem(STORAGE_KEY, JSON.stringify([...new Set(ids)]));
  window.dispatchEvent(new Event(CHANGE_EVENT));
}
