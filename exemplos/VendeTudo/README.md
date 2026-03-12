# VendeTudo — Loja Virtual de Referência

Aplicação de e-commerce de referência com arquitetura de microsserviços.

**Stack:** .NET 10 + Next.js 15 + React 19 + TypeScript + Tailwind CSS + shadcn/ui

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 22+](https://nodejs.org/)
- [Docker](https://www.docker.com/) + Docker Compose
- [.NET Aspire workload](https://learn.microsoft.com/aspire/): `dotnet workload install aspire`

---

## Rodar com Docker Compose

```bash
cd exemplos/VendeTudo
docker-compose up --build
```

Serviços disponíveis:

| Serviço | URL |
|---------|-----|
| Frontend (Next.js) | http://localhost:3000 |
| Identidade API | http://localhost:5001 |
| Catálogo API | http://localhost:5002 |
| Cesta API | http://localhost:5003 |
| Pedidos API | http://localhost:5004 |
| RabbitMQ Management | http://localhost:15672 |

**Credenciais padrão:**
- `admin@vendetudo.com` / `Admin@123`
- `alice@vendetudo.com` / `Pass@123`

---

## Rodar com .NET Aspire

```bash
cd exemplos/VendeTudo
dotnet run --project src/VendeTudo.AppHost
```

O painel do Aspire abrirá automaticamente no browser.

---

## Rodar o Frontend localmente

```bash
cd exemplos/VendeTudo/src/Web
npm install
npm run dev
```

O frontend estará disponível em http://localhost:3000.

---

## Executar os Testes

```bash
cd exemplos/VendeTudo
dotnet test
```

---

## Estrutura do Projeto

```
exemplos/VendeTudo/
├── VendeTudo.sln
├── Directory.Build.props         # TargetFramework=net10.0
├── Directory.Packages.props      # Central Package Management
├── docker-compose.yml
├── docker-compose.override.yml
│
├── src/
│   ├── VendeTudo.AppHost/        # .NET Aspire AppHost
│   ├── VendeTudo.PadroeServico/  # Aspire Service Defaults
│   ├── VendeTudo.Compartilhado/  # DTOs, eventos, interfaces
│   ├── BarramentoEventos/        # Interface IBarramentoEventos
│   ├── BarramentoEventosRabbitMQ/# Implementação RabbitMQ
│   ├── Identidade.API/           # Identity + OpenIddict (porta 5001)
│   ├── Catalogo.API/             # Catálogo de produtos (porta 5002)
│   ├── Cesta.API/                # Carrinho Redis (porta 5003)
│   ├── Pedidos.API/              # Gestão de pedidos (porta 5004)
│   ├── Pedidos.Dominio/          # DDD: Agregados, Value Objects
│   ├── Pedidos.Infraestrutura/   # EF Core + PostgreSQL
│   ├── ProcessadorPedidos/       # Worker: avança status de pedidos
│   ├── ProcessadorPagamentos/    # Worker: simula aprovação de pagamentos
│   └── Web/                      # Frontend Next.js 15 (porta 3000)
│
└── testes/
    ├── Catalogo.API.Testes/
    ├── Pedidos.Dominio.Testes/
    └── Pedidos.API.Testes/
```

---

## Arquitetura

```
┌─────────────────────────────────────────────────────────┐
│                   Next.js Frontend                      │
│         (catálogo, cesta, checkout, pedidos)            │
└─────────────┬──────────────────────────────────┬────────┘
              │ HTTP/REST                        │ HTTP/REST
              ▼                                  ▼
┌─────────────────────┐              ┌─────────────────────┐
│    Identidade.API   │              │    Catalogo.API     │
│   (OpenIddict OIDC) │              │  (produtos, preços) │
└─────────────────────┘              └─────────────────────┘
              │                                  │
              ▼                                  ▼
┌─────────────────────┐    RabbitMQ   ┌──────────────────┐
│      Cesta.API      │◄─────────────►│   Pedidos.API    │
│       (Redis)       │               │  (CQRS/MediatR)  │
└─────────────────────┘               └──────────────────┘
                                              │
                              ┌───────────────┼────────────────┐
                              ▼               ▼                ▼
                   ┌──────────────┐  ┌─────────────┐  ┌────────────┐
                   │ Processador  │  │ Processador │  │ PostgreSQL │
                   │  Pedidos     │  │ Pagamentos  │  │   (dados)  │
                   └──────────────┘  └─────────────┘  └────────────┘
```

---

## Validação Pré-PR

| Etapa | Status |
|-------|--------|
| `dotnet restore` | ✅ OK |
| `dotnet build` | ✅ OK |
| `dotnet test` | ✅ 31 passed |
