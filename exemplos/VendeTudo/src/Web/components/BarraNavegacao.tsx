import Link from "next/link";
import { ShoppingCart, User, Package } from "lucide-react";
import { Button } from "@/components/ui/button";

export function BarraNavegacao() {
  return (
    <header className="border-b bg-white sticky top-0 z-50">
      <div className="container mx-auto px-4 h-16 flex items-center justify-between">
        <Link href="/" className="text-2xl font-bold text-primary">
          VendeTudo
        </Link>
        <nav className="flex items-center gap-4">
          <Link href="/catalogo" className="text-sm font-medium hover:text-primary transition-colors">
            Catálogo
          </Link>
          <Link href="/pedidos" className="text-sm font-medium hover:text-primary transition-colors">
            <Package className="h-4 w-4 inline mr-1" />
            Pedidos
          </Link>
          <Link href="/cesta">
            <Button variant="outline" size="sm">
              <ShoppingCart className="h-4 w-4 mr-1" />
              Cesta
            </Button>
          </Link>
          <Link href="/usuario">
            <Button variant="ghost" size="icon">
              <User className="h-4 w-4" />
            </Button>
          </Link>
        </nav>
      </div>
    </header>
  );
}
