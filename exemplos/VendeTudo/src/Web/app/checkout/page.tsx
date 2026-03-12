"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { useRouter } from "next/navigation";

export default function PaginaCheckout() {
  const router = useRouter();
  const [form, setForm] = useState({
    rua: "",
    cidade: "",
    estado: "",
    pais: "Brasil",
    cep: "",
  });
  const [enviando, setEnviando] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setEnviando(true);
    // Simula confirmação do pedido
    setTimeout(() => {
      localStorage.removeItem("cesta");
      router.push("/pedidos");
    }, 1000);
  };

  return (
    <div className="max-w-lg mx-auto">
      <h1 className="text-3xl font-bold mb-6">Finalizar Compra</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block text-sm font-medium mb-1">Rua</label>
          <input
            name="rua"
            value={form.rua}
            onChange={handleChange}
            required
            className="w-full border rounded-md px-3 py-2 text-sm"
            placeholder="Rua das Flores, 123"
          />
        </div>
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium mb-1">Cidade</label>
            <input
              name="cidade"
              value={form.cidade}
              onChange={handleChange}
              required
              className="w-full border rounded-md px-3 py-2 text-sm"
            />
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Estado</label>
            <input
              name="estado"
              value={form.estado}
              onChange={handleChange}
              required
              className="w-full border rounded-md px-3 py-2 text-sm"
            />
          </div>
        </div>
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium mb-1">País</label>
            <input
              name="pais"
              value={form.pais}
              onChange={handleChange}
              required
              className="w-full border rounded-md px-3 py-2 text-sm"
            />
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">CEP</label>
            <input
              name="cep"
              value={form.cep}
              onChange={handleChange}
              required
              className="w-full border rounded-md px-3 py-2 text-sm"
              placeholder="00000-000"
            />
          </div>
        </div>
        <Button type="submit" size="lg" className="w-full" disabled={enviando}>
          {enviando ? "Confirmando pedido..." : "Confirmar Pedido"}
        </Button>
      </form>
    </div>
  );
}
