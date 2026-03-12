import { obterPedido } from "@/src/servicos/pedidosServico";
import { notFound } from "next/navigation";
import { BadgeStatusPedido } from "@/components/BadgeStatusPedido";

export default async function PaginaDetalhePedido({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  const pedido = await obterPedido(Number(id));
  if (!pedido) notFound();

  return (
    <div className="max-w-3xl mx-auto">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-3xl font-bold">Pedido #{pedido.id}</h1>
        <BadgeStatusPedido status={pedido.status} />
      </div>
      <div className="border rounded-lg p-4 mb-4">
        <h2 className="font-semibold mb-2">Endereço de Entrega</h2>
        <p className="text-sm text-muted-foreground">
          {pedido.endereco?.rua}, {pedido.endereco?.cidade} — {pedido.endereco?.estado}, {pedido.endereco?.pais} — CEP: {pedido.endereco?.cep}
        </p>
      </div>
      <div className="border rounded-lg p-4">
        <h2 className="font-semibold mb-4">Itens</h2>
        <div className="space-y-2">
          {pedido.itens?.map((item, i) => (
            <div key={i} className="flex justify-between text-sm">
              <span>{item.nomeProduto} × {item.unidades}</span>
              <span>{new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(item.precoUnitario * item.unidades)}</span>
            </div>
          ))}
        </div>
        <div className="border-t mt-4 pt-4 flex justify-between font-bold">
          <span>Total</span>
          <span>{new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(pedido.total)}</span>
        </div>
      </div>
    </div>
  );
}
