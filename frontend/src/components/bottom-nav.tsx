import { navItems } from "@/components/nav-items";
import { cn } from "@/lib/utils";

export function BottomNav() {
  return (
    <nav className="fixed inset-x-0 bottom-0 z-10 flex justify-around border-t bg-white px-4 py-2 md:hidden">
      {navItems.map(({ label, icon: Icon, active }) => (
        <button
          key={label}
          className={cn(
            "flex w-24 flex-col items-center gap-1 rounded-xl py-2 text-xs",
            active
              ? "bg-orange-soft font-semibold text-orange"
              : "text-neutral-600"
          )}
        >
          <Icon className="size-5" />
          {label}
        </button>
      ))}
    </nav>
  );
}