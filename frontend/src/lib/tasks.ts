import { api } from "@/lib/api";

export async function finishTasks(taskIds: string[]): Promise<void> {
  // Finish sequentially so the backend sees earlier task updates before deciding
  // whether the last task completes its order.
  for (const taskId of taskIds) {
    await api.post(`/api/Task/finish/${encodeURIComponent(taskId)}`);
  }
}
