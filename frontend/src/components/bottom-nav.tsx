"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { navItems } from "@/components/nav-items";
import { cn } from "@/lib/utils";

export function BottomNav() {
  const pathname = usePathname();

  return (
    <nav className="fixed inset-x-0 bottom-0 z-10 flex justify-around border-t bg-white px-4 py-2 md:hidden">
      {navItems.map(({ label, icon: Icon, href }) => {
        const className = cn(
          "flex w-24 flex-col items-center gap-1 rounded-xl py-2 text-xs",
          href === pathname
            ? "bg-orange-soft font-semibold text-orange"
            : "text-neutral-600"
        );
        const content = (
          <>
            <Icon className="size-5" />
            {label}
          </>
        );

        return href ? (
          <Link key={label} href={href} className={className}>
            {content}
          </Link>
        ) : (
          <button key={label} type="button" className={className}>
            {content}
          </button>
        );
      })}
    </nav>
  );
}