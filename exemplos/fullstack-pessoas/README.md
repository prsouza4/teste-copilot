# Fullstack Pessoas — Exemplo Completo

Aplicação de exemplo demonstrando integração entre:

- **API.Auth** — Duende IdentityServer com configuração in-memory
- **API.Cadastro** — ASP.NET Core Web API com CRUD de Pessoas protegida por JWT
- **Frontend** — Next.js 15 com login via OAuth2/OIDC e CRUD de Pessoas

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 22+](https://nodejs.org/)
- [Docker](https://www.docker.com/) e Docker Compose (opcional, para rodar tudo de uma vez)

## Rodando com Docker Compose

```bash
cd exemplos/fullstack-pessoas
docker-compose up --build
```

Aguarde todos os serviços iniciarem. Acesse:

- **Frontend**: http://localhost:3000
- **API.Auth** (IdentityServer): http://localhost:5001
- **API.Cadastro** (Swagger): http://localhost:5002/swagger

Login padrão: `admin@exemplo.com` / `Admin@123`

## Rodando Localmente (sem Docker)

### 1. API.Auth (porta 5001)

```bash
cd exemplos/fullstack-pessoas/src/API.Auth
dotnet run
```

### 2. API.Cadastro (porta 5002)

```bash
cd exemplos/fullstack-pessoas/src/API.Cadastro
dotnet run
```

### 3. Frontend (porta 3000)

```bash
cd exemplos/fullstack-pessoas/src/frontend
cp .env.local.example .env.local
# Edite .env.local se necessário
npm install
npm run dev
```

## Rodando os Testes

```bash
cd exemplos/fullstack-pessoas
dotnet test
```

Resultado esperado: 13 testes passando (6 em API.Auth.Tests + 7 em API.Cadastro.Tests).

## Estrutura do Projeto

```
exemplos/fullstack-pessoas/
├── src/
│   ├── API.Auth/               # Duende IdentityServer (porta 5001)
│   │   ├── Config.cs           # Clients, Resources, Scopes in-memory
│   │   └── SeedData.cs         # Usuário: admin@exemplo.com / Admin@123
│   ├── API.Cadastro/           # Web API com CRUD (porta 5002)
│   │   ├── Controllers/        # PessoasController
│   │   ├── Models/             # Pessoa
│   │   └── Data/               # EF Core InMemory
│   └── frontend/               # Next.js 15 (porta 3000)
│       └── src/app/            # App Router
│           ├── login/          # Página de login
│           └── pessoas/        # CRUD de pessoas
├── tests/
│   ├── API.Auth.Tests/         # Testes xUnit para API.Auth
│   └── API.Cadastro.Tests/     # Testes xUnit para API.Cadastro
├── docker-compose.yml
└── README.md
```

## Tecnologias

| Camada | Tecnologia |
|--------|-----------|
| Autenticação | Duende IdentityServer 7.x + ASP.NET Core Identity |
| API | ASP.NET Core 10 + EF Core InMemory |
| Frontend | Next.js 15 + React 19 + TypeScript |
| Estilo | Tailwind CSS 3 |
| Auth Frontend | next-auth v5 (Auth.js) |
| Testes | xUnit + WebApplicationFactory |
| Container | Docker + Docker Compose |

## Usuário Padrão

| Campo | Valor |
|-------|-------|
| Email | admin@exemplo.com |
| Senha | Admin@123 |

## Endpoints da API.Cadastro

| Método | Rota | Escopo Requerido |
|--------|------|-----------------|
| GET | /api/pessoas | api.cadastro:read |
| GET | /api/pessoas/{id} | api.cadastro:read |
| POST | /api/pessoas | api.cadastro:write |
| PUT | /api/pessoas/{id} | api.cadastro:write |
| DELETE | /api/pessoas/{id} | api.cadastro:write |

Todas as rotas requerem autenticação via Bearer Token (JWT emitido pela API.Auth).
