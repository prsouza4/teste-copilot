"use client";

export default function PaginaUsuario() {
  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-3xl font-bold mb-6">Minha Conta</h1>
      <div className="border rounded-lg p-6 space-y-4">
        <div>
          <p className="text-sm font-medium text-muted-foreground">Nome</p>
          <p className="text-lg">Usuário VendeTudo</p>
        </div>
        <div>
          <p className="text-sm font-medium text-muted-foreground">E-mail</p>
          <p className="text-lg">alice@vendetudo.com</p>
        </div>
        <div>
          <p className="text-sm font-medium text-muted-foreground">Papel</p>
          <p className="text-lg">Cliente</p>
        </div>
      </div>
    </div>
  );
}
