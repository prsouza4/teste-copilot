"use client";

interface FiltrosCatalogoProps {
  tipos: { id: number; tipo: string }[];
  marcas: { id: number; marca: string }[];
  tipoProdutoSelecionado: string;
  marcaProdutoSelecionada: string;
  onFiltrar: (tipoProduto: string, marcaProduto: string) => void;
}

export function FiltrosCatalogo({
  tipos,
  marcas,
  tipoProdutoSelecionado,
  marcaProdutoSelecionada,
  onFiltrar,
}: FiltrosCatalogoProps) {
  return (
    <div className="flex gap-4 flex-wrap mb-6">
      <select
        value={tipoProdutoSelecionado}
        onChange={(e) => onFiltrar(e.target.value, marcaProdutoSelecionada)}
        className="border rounded-md px-3 py-2 text-sm bg-white"
      >
        <option value="">Todos os tipos</option>
        {tipos.map((t) => (
          <option key={t.id} value={String(t.id)}>
            {t.tipo}
          </option>
        ))}
      </select>
      <select
        value={marcaProdutoSelecionada}
        onChange={(e) => onFiltrar(tipoProdutoSelecionado, e.target.value)}
        className="border rounded-md px-3 py-2 text-sm bg-white"
      >
        <option value="">Todas as marcas</option>
        {marcas.map((m) => (
          <option key={m.id} value={String(m.id)}>
            {m.marca}
          </option>
        ))}
      </select>
    </div>
  );
}
