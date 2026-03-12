import Link from "next/link";
import { ShoppingCart } from "lucide-react";
import { Button } from "@/components/ui/button";

interface ResumoCestaProps {
  totalItens: number;
  total: number;
}

export function ResumoCesta({ totalItens, total }: ResumoCestaProps) {
  return (
    <div className="flex items-center gap-2">
      <Link href="/cesta">
        <Button variant="outline" size="sm">
          <ShoppingCart className="h-4 w-4 mr-1" />
          {totalItens} {totalItens === 1 ? "item" : "itens"}
          {" · "}
          {new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(total)}
        </Button>
      </Link>
    </div>
  );
}
