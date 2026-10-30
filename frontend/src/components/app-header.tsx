import { Warehouse } from "lucide-react";

export function AppHeader() {
  return (
    <header className="sticky top-0 z-10 flex h-12 items-center justify-between bg-black px-4 text-white md:justify-end md:px-8">
      <div className="flex items-center gap-2 font-semibold md:hidden">
        <Warehouse className="size-4" />
        SmartPick
      </div>
    </header>
  );
}