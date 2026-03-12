const URL_PEDIDOS = process.env.NEXT_PUBLIC_URL_PEDIDOS ?? "http://localhost:5004";

export interface ItemPedido {
  idProduto: number;
  nomeProduto: string;
  precoUnitario: number;
  unidades: number;
  urlImagem: string;
}

export interface Pedido {
  id: number;
  idComprador: string;
  status: number;
  total: number;
  endereco: {
    rua: string;
    cidade: string;
    estado: string;
    pais: string;
    cep: string;
  };
  itens: ItemPedido[];
}

export async function obterPedidos(token?: string): Promise<Pedido[]> {
  const headers: Record<string, string> = {};
  if (token) headers["Authorization"] = `Bearer ${token}`;
  const res = await fetch(`${URL_PEDIDOS}/api/pedidos`, { headers, cache: "no-store" });
  if (!res.ok) return [];
  return res.json();
}

export async function obterPedido(id: number, token?: string): Promise<Pedido | null> {
  const headers: Record<string, string> = {};
  if (token) headers["Authorization"] = `Bearer ${token}`;
  const res = await fetch(`${URL_PEDIDOS}/api/pedidos/${id}`, { headers, cache: "no-store" });
  if (!res.ok) return null;
  return res.json();
}
