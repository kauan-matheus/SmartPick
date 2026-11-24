import type { TaskType } from "@/lib/picking";

export interface CollectionReviewData {
  collected: number[];
  justified: Record<string, string>;
}

const collectionReviewUpdatedEvent = "smartpick:collection-review-updated";

export function getCollectionReviewStorageKey(
  orderId: number,
  taskType: TaskType
) {
  return `smartpick-review-${orderId}-${taskType}`;
}

export function notifyCollectionReviewUpdated() {
  window.dispatchEvent(new Event(collectionReviewUpdatedEvent));
}

export function subscribeToCollectionReview(callback: () => void) {
  window.addEventListener("storage", callback);
  window.addEventListener(collectionReviewUpdatedEvent, callback);

  return () => {
    window.removeEventListener("storage", callback);
    window.removeEventListener(collectionReviewUpdatedEvent, callback);
  };
}
