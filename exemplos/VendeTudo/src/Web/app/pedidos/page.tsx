"use client";

import { useState, useEffect } from "react";
import { BadgeStatusPedido } from "@/components/BadgeStatusPedido";
import Link from "next/link";
import type { Pedido } from "@/src/servicos/pedidosServico";

export default function PaginaPedidos() {
  const [pedidos, setPedidos] = useState<Pedido[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    fetch(`${process.env.NEXT_PUBLIC_URL_PEDIDOS ?? "http://localhost:5004"}/api/pedidos`, { cache: "no-store" })
      .then((r) => r.ok ? r.json() : [])
      .then(setPedidos)
      .catch(() => setPedidos([]))
      .finally(() => setCarregando(false));
  }, []);

  if (carregando) {
    return <div className="text-center py-12">Carregando pedidos...</div>;
  }

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Meus Pedidos</h1>
      {pedidos.length === 0 ? (
        <div className="text-center py-12 text-muted-foreground">
          Você ainda não tem pedidos.
        </div>
      ) : (
        <div className="space-y-4">
          {pedidos.map((pedido) => (
            <Link key={pedido.id} href={`/pedidos/${pedido.id}`}>
              <div className="border rounded-lg p-4 flex items-center justify-between hover:bg-muted/50 transition-colors">
                <div>
                  <p className="font-medium">Pedido #{pedido.id}</p>
                  <p className="text-sm text-muted-foreground">
                    {new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(pedido.total)}
                    {" · "}
                    {pedido.itens?.length ?? 0} {(pedido.itens?.length ?? 0) === 1 ? "item" : "itens"}
                  </p>
                </div>
                <BadgeStatusPedido status={pedido.status} />
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
