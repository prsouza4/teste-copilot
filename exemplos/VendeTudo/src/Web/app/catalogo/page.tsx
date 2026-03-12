"use client";

import { useState, useEffect } from "react";
import { CartaoProduto } from "@/components/CartaoProduto";
import { FiltrosCatalogo } from "@/components/FiltrosCatalogo";
import { PaginacaoCatalogo } from "@/components/PaginacaoCatalogo";
import { listarItens, listarTipos, listarMarcas, type ItemCatalogo } from "@/src/servicos/catalogoServico";

const TAMANHO_PAGINA = 8;

export default function PaginaCatalogo() {
  const [itens, setItens] = useState<ItemCatalogo[]>([]);
  const [tipos, setTipos] = useState<{ id: number; tipo: string }[]>([]);
  const [marcas, setMarcas] = useState<{ id: number; marca: string }[]>([]);
  const [pagina, setPagina] = useState(1);
  const [total, setTotal] = useState(0);
  const [tipoProduto, setTipoProduto] = useState("");
  const [marcaProduto, setMarcaProduto] = useState("");
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    listarTipos().then(setTipos);
    listarMarcas().then(setMarcas);
  }, []);

  useEffect(() => {
    setCarregando(true);
    listarItens(pagina, TAMANHO_PAGINA, tipoProduto, marcaProduto)
      .then((res) => {
        setItens(res.itens ?? []);
        setTotal(res.total ?? 0);
      })
      .catch(() => setItens([]))
      .finally(() => setCarregando(false));
  }, [pagina, tipoProduto, marcaProduto]);

  const totalPaginas = Math.ceil(total / TAMANHO_PAGINA) || 1;

  const handleFiltrar = (novoTipo: string, novaMarca: string) => {
    setTipoProduto(novoTipo);
    setMarcaProduto(novaMarca);
    setPagina(1);
  };

  const handleAdicionarCesta = (item: ItemCatalogo) => {
    // Implementação simplificada — salva no localStorage
    const cestaStr = localStorage.getItem("cesta");
    const cesta = cestaStr ? JSON.parse(cestaStr) : { itens: [] };
    const existente = cesta.itens.find((i: any) => i.idProduto === item.id);
    if (existente) {
      existente.quantidade += 1;
    } else {
      cesta.itens.push({
        id: item.id,
        nomeProduto: item.nome,
        precoAntigo: item.preco,
        precoUnitario: item.preco,
        quantidade: 1,
        urlImagem: item.urlImagem,
        idProduto: item.id,
      });
    }
    localStorage.setItem("cesta", JSON.stringify(cesta));
    alert(`${item.nome} adicionado à cesta!`);
  };

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Catálogo</h1>
      <FiltrosCatalogo
        tipos={tipos}
        marcas={marcas}
        tipoProdutoSelecionado={tipoProduto}
        marcaProdutoSelecionada={marcaProduto}
        onFiltrar={handleFiltrar}
      />
      {carregando ? (
        <div className="text-center py-12">Carregando produtos...</div>
      ) : itens.length === 0 ? (
        <div className="text-center py-12 text-muted-foreground">Nenhum produto encontrado.</div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {itens.map((item) => (
            <CartaoProduto key={item.id} item={item} onAdicionarCesta={handleAdicionarCesta} />
          ))}
        </div>
      )}
      <PaginacaoCatalogo
        paginaAtual={pagina}
        totalPaginas={totalPaginas}
        onMudarPagina={setPagina}
      />
    </div>
  );
}
