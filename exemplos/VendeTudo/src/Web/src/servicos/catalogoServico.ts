const URL_CATALOGO = process.env.NEXT_PUBLIC_URL_CATALOGO ?? "http://localhost:5002";

export interface ItemCatalogo {
  id: number;
  nome: string;
  descricao: string;
  preco: number;
  urlImagem: string;
  quantidadeEstoque: number;
  tipoProdutoId: number;
  tipoProduto: { id: number; tipo: string };
  marcaProdutoId: number;
  marcaProduto: { id: number; marca: string };
}

export interface RespostaPaginada {
  pagina: number;
  tamanhoPagina: number;
  total: number;
  itens: ItemCatalogo[];
}

export async function listarItens(pagina = 1, tamanhoPagina = 10, tipoProduto = "", marcaProduto = ""): Promise<RespostaPaginada> {
  const params = new URLSearchParams({ pagina: String(pagina), tamanhoPagina: String(tamanhoPagina) });
  if (tipoProduto) params.set("tipoProduto", tipoProduto);
  if (marcaProduto) params.set("marcaProduto", marcaProduto);
  const res = await fetch(`${URL_CATALOGO}/api/catalogo/itens?${params}`, { cache: "no-store" });
  if (!res.ok) throw new Error("Erro ao listar itens");
  return res.json();
}

export async function obterItem(id: number): Promise<ItemCatalogo> {
  const res = await fetch(`${URL_CATALOGO}/api/catalogo/itens/${id}`, { cache: "no-store" });
  if (!res.ok) throw new Error("Item não encontrado");
  return res.json();
}

export async function listarTipos(): Promise<{ id: number; tipo: string }[]> {
  const res = await fetch(`${URL_CATALOGO}/api/catalogo/tipos-produto`, { cache: "no-store" });
  if (!res.ok) return [];
  return res.json();
}

export async function listarMarcas(): Promise<{ id: number; marca: string }[]> {
  const res = await fetch(`${URL_CATALOGO}/api/catalogo/marcas-produto`, { cache: "no-store" });
  if (!res.ok) return [];
  return res.json();
}
