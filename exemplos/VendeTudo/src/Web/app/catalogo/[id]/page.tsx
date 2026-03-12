import { obterItem } from "@/src/servicos/catalogoServico";
import { notFound } from "next/navigation";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { ShoppingCart } from "lucide-react";

export default async function PaginaDetalheItem({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  let item;
  try {
    item = await obterItem(Number(id));
  } catch {
    notFound();
  }

  return (
    <div className="max-w-4xl mx-auto">
      <div className="grid md:grid-cols-2 gap-8">
        <div className="aspect-square bg-gray-100 rounded-lg overflow-hidden">
          {item.urlImagem ? (
            <img src={item.urlImagem} alt={item.nome} className="w-full h-full object-cover" />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-gray-400">
              Sem imagem
            </div>
          )}
        </div>
        <div className="flex flex-col gap-4">
          <h1 className="text-3xl font-bold">{item.nome}</h1>
          <div className="flex gap-2">
            <Badge variant="outline">{item.tipoProduto?.tipo}</Badge>
            <Badge variant="secondary">{item.marcaProduto?.marca}</Badge>
          </div>
          <p className="text-muted-foreground">{item.descricao}</p>
          <div className="text-4xl font-bold text-primary">
            {new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(item.preco)}
          </div>
          <p className="text-sm text-muted-foreground">
            {item.quantidadeEstoque > 0
              ? `${item.quantidadeEstoque} em estoque`
              : "Fora de estoque"}
          </p>
          <Button size="lg" disabled={item.quantidadeEstoque === 0}>
            <ShoppingCart className="h-5 w-5 mr-2" />
            Adicionar à Cesta
          </Button>
        </div>
      </div>
    </div>
  );
}
