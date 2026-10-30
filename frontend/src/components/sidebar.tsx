import { Warehouse } from "lucide-react";
import { navItems } from "@/components/nav-items";
import { cn } from "@/lib/utils";

export function Sidebar() {
  return (
    <aside className="sticky top-0 hidden h-screen w-60 shrink-0 self-start border-r bg-white md:flex md:flex-col">
      <div className="flex h-12 items-center gap-2 bg-black px-4 font-semibold text-white">
        <Warehouse className="size-4" />
        elo
      </div>

      <nav className="flex flex-col gap-1 p-3">
        {navItems.map(({ label, icon: Icon, active }) => (
          <button
            key={label}
            className={cn(
              "flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm transition-colors",
              active
                ? "bg-orange-soft font-semibold text-orange"
                : "text-neutral-600 hover:bg-neutral-100"
            )}
          >
            <Icon className="size-5" />
            {label}
          </button>
        ))}
      </nav>
    </aside>
  );
}