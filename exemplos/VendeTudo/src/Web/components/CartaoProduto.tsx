"use client";

import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { ShoppingCart } from "lucide-react";
import type { ItemCatalogo } from "@/src/servicos/catalogoServico";
import Link from "next/link";

interface CartaoProdutoProps {
  item: ItemCatalogo;
  onAdicionarCesta?: (item: ItemCatalogo) => void;
}

export function CartaoProduto({ item, onAdicionarCesta }: CartaoProdutoProps) {
  return (
    <Card className="flex flex-col h-full">
      <CardHeader>
        <Link href={`/catalogo/${item.id}`}>
          <div className="aspect-square bg-gray-100 rounded-md mb-2 overflow-hidden">
            {item.urlImagem ? (
              <img
                src={item.urlImagem}
                alt={item.nome}
                className="w-full h-full object-cover"
              />
            ) : (
              <div className="w-full h-full flex items-center justify-center text-gray-400">
                Sem imagem
              </div>
            )}
          </div>
          <CardTitle className="text-base hover:underline">{item.nome}</CardTitle>
        </Link>
        <div className="flex gap-2 flex-wrap">
          <Badge variant="outline">{item.tipoProduto?.tipo}</Badge>
          <Badge variant="secondary">{item.marcaProduto?.marca}</Badge>
        </div>
      </CardHeader>
      <CardContent className="flex-1">
        <p className="text-sm text-muted-foreground line-clamp-2">{item.descricao}</p>
      </CardContent>
      <CardFooter className="flex items-center justify-between">
        <span className="text-xl font-bold text-primary">
          {new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(item.preco)}
        </span>
        {onAdicionarCesta && (
          <Button size="sm" onClick={() => onAdicionarCesta(item)}>
            <ShoppingCart className="h-4 w-4 mr-1" />
            Adicionar
          </Button>
        )}
      </CardFooter>
    </Card>
  );
}
