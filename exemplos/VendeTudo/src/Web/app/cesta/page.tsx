"use client";

import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Trash2, Plus, Minus } from "lucide-react";
import Link from "next/link";

interface ItemCesta {
  id: number;
  nomeProduto: string;
  precoUnitario: number;
  quantidade: number;
  urlImagem: string;
  idProduto: number;
}

export default function PaginaCesta() {
  const [itens, setItens] = useState<ItemCesta[]>([]);

  useEffect(() => {
    const cestaStr = localStorage.getItem("cesta");
    if (cestaStr) {
      const cesta = JSON.parse(cestaStr);
      setItens(cesta.itens ?? []);
    }
  }, []);

  const salvarCesta = (novosItens: ItemCesta[]) => {
    setItens(novosItens);
    localStorage.setItem("cesta", JSON.stringify({ itens: novosItens }));
  };

  const alterarQuantidade = (id: number, delta: number) => {
    const novosItens = itens
      .map((i) => i.id === id ? { ...i, quantidade: i.quantidade + delta } : i)
      .filter((i) => i.quantidade > 0);
    salvarCesta(novosItens);
  };

  const removerItem = (id: number) => {
    salvarCesta(itens.filter((i) => i.id !== id));
  };

  const total = itens.reduce((s, i) => s + i.precoUnitario * i.quantidade, 0);
  const formatarMoeda = (v: number) =>
    new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(v);

  if (itens.length === 0) {
    return (
      <div className="text-center py-16">
        <h1 className="text-3xl font-bold mb-4">Cesta Vazia</h1>
        <p className="text-muted-foreground mb-6">Você ainda não adicionou itens à cesta.</p>
        <Link href="/catalogo">
          <Button>Ver Catálogo</Button>
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-3xl font-bold mb-6">Minha Cesta</h1>
      <div className="space-y-4">
        {itens.map((item) => (
          <div key={item.id} className="flex items-center gap-4 border rounded-lg p-4">
            <div className="w-16 h-16 bg-gray-100 rounded overflow-hidden shrink-0">
              {item.urlImagem && <img src={item.urlImagem} alt={item.nomeProduto} className="w-full h-full object-cover" />}
            </div>
            <div className="flex-1">
              <p className="font-medium">{item.nomeProduto}</p>
              <p className="text-sm text-muted-foreground">{formatarMoeda(item.precoUnitario)} cada</p>
            </div>
            <div className="flex items-center gap-2">
              <Button variant="outline" size="icon" onClick={() => alterarQuantidade(item.id, -1)}>
                <Minus className="h-4 w-4" />
              </Button>
              <span className="w-8 text-center">{item.quantidade}</span>
              <Button variant="outline" size="icon" onClick={() => alterarQuantidade(item.id, 1)}>
                <Plus className="h-4 w-4" />
              </Button>
            </div>
            <p className="w-24 text-right font-medium">{formatarMoeda(item.precoUnitario * item.quantidade)}</p>
            <Button variant="ghost" size="icon" onClick={() => removerItem(item.id)}>
              <Trash2 className="h-4 w-4 text-destructive" />
            </Button>
          </div>
        ))}
      </div>
      <div className="mt-6 border-t pt-4 flex justify-between items-center">
        <span className="text-xl font-bold">Total: {formatarMoeda(total)}</span>
        <Link href="/checkout">
          <Button size="lg">Finalizar Compra</Button>
        </Link>
      </div>
    </div>
  );
}
