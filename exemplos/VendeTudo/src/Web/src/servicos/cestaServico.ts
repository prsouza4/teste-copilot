const URL_CESTA = process.env.NEXT_PUBLIC_URL_CESTA ?? "http://localhost:5003";

export interface ItemCesta {
  id: number;
  nomeProduto: string;
  precoAntigo: number;
  precoUnitario: number;
  quantidade: number;
  urlImagem: string;
  idProduto: number;
}

export interface CestaCliente {
  idComprador: string;
  itens: ItemCesta[];
  total: number;
}

export async function obterCesta(idComprador: string, token?: string): Promise<CestaCliente> {
  const headers: Record<string, string> = {};
  if (token) headers["Authorization"] = `Bearer ${token}`;
  const res = await fetch(`${URL_CESTA}/api/cesta/${idComprador}`, { headers, cache: "no-store" });
  if (!res.ok) return { idComprador, itens: [], total: 0 };
  return res.json();
}

export async function atualizarCesta(cesta: CestaCliente, token?: string): Promise<CestaCliente> {
  const headers: Record<string, string> = { "Content-Type": "application/json" };
  if (token) headers["Authorization"] = `Bearer ${token}`;
  const res = await fetch(`${URL_CESTA}/api/cesta`, { method: "POST", headers, body: JSON.stringify(cesta), cache: "no-store" });
  if (!res.ok) throw new Error("Erro ao atualizar cesta");
  return res.json();
}

export interface EnderecoCheckout {
  rua: string;
  cidade: string;
  estado: string;
  pais: string;
  cep: string;
}

export async function checkout(idComprador: string, endereco: EnderecoCheckout, token?: string): Promise<void> {
  const headers: Record<string, string> = { "Content-Type": "application/json" };
  if (token) headers["Authorization"] = `Bearer ${token}`;
  const res = await fetch(`${URL_CESTA}/api/cesta/checkout`, {
    method: "POST",
    headers,
    body: JSON.stringify({ idComprador, endereco }),
    cache: "no-store",
  });
  if (!res.ok) throw new Error("Erro no checkout");
}
