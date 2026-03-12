"use client";
import { Button } from "@/components/ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";

interface PaginacaoCatalogoProps {
  paginaAtual: number;
  totalPaginas: number;
  onMudarPagina: (pagina: number) => void;
}

export function PaginacaoCatalogo({ paginaAtual, totalPaginas, onMudarPagina }: PaginacaoCatalogoProps) {
  return (
    <div className="flex items-center gap-2 justify-center mt-8">
      <Button
        variant="outline"
        size="icon"
        disabled={paginaAtual <= 1}
        onClick={() => onMudarPagina(paginaAtual - 1)}
      >
        <ChevronLeft className="h-4 w-4" />
      </Button>
      <span className="text-sm">
        Página {paginaAtual} de {totalPaginas}
      </span>
      <Button
        variant="outline"
        size="icon"
        disabled={paginaAtual >= totalPaginas}
        onClick={() => onMudarPagina(paginaAtual + 1)}
      >
        <ChevronRight className="h-4 w-4" />
      </Button>
    </div>
  );
}
