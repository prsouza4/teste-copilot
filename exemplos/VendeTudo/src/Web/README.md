# VendeTudo - Frontend Web

Frontend da aplicação VendeTudo construído com Next.js 15, React 19, TypeScript, Tailwind CSS e shadcn/ui.

## Stack Tecnológico

- **Framework**: Next.js 15 (App Router)
- **UI Library**: React 19
- **Linguagem**: TypeScript (strict mode)
- **Styling**: Tailwind CSS
- **Componentes**: shadcn/ui (Radix UI primitives)
- **Autenticação**: NextAuth.js v5 (OpenID Connect)
- **Ícones**: Lucide React

## Estrutura do Projeto

```
Web/
├── app/                          # Rotas do Next.js (App Router)
│   ├── catalogo/                 # Listagem e detalhes de produtos
│   ├── cesta/                    # Carrinho de compras
│   ├── checkout/                 # Finalização de pedido
│   ├── pedidos/                  # Histórico de pedidos
│   ├── usuario/                  # Perfil do usuário
│   ├── api/auth/                 # Rotas de autenticação
│   ├── layout.tsx                # Layout global
│   ├── page.tsx                  # Página inicial (redirect)
│   └── globals.css               # Estilos globais
├── components/                   # Componentes React
│   ├── ui/                       # Componentes shadcn/ui (não editar)
│   ├── BarraNavegacao.tsx        # Header com navegação
│   ├── CartaoProduto.tsx         # Card de produto
│   ├── BadgeStatusPedido.tsx     # Badge de status de pedido
│   ├── FiltrosCatalogo.tsx       # Filtros de busca
│   ├── PaginacaoCatalogo.tsx     # Controles de paginação
│   └── ResumoCesta.tsx           # Resumo do carrinho
├── lib/                          # Utilitários
│   ├── auth.ts                   # Configuração NextAuth
│   └── utils.ts                  # Função cn() e helpers
├── src/servicos/                 # Camada de serviços/API
│   ├── catalogoServico.ts        # API de catálogo
│   ├── cestaServico.ts           # API de cesta
│   └── pedidosServico.ts         # API de pedidos
├── package.json                  # Dependências
├── tsconfig.json                 # Configuração TypeScript
├── tailwind.config.ts            # Configuração Tailwind
├── next.config.ts                # Configuração Next.js
├── Dockerfile                    # Build para produção
└── .env.local                    # Variáveis de ambiente
```

## Pré-requisitos

- Node.js 22+
- npm ou yarn

## Instalação e Execução

### 1. Instalar dependências

```bash
npm install
```

### 2. Configurar variáveis de ambiente

Edite o arquivo `.env.local` com as URLs dos serviços:

```env
AUTH_SECRET=seu-segredo-aqui
AUTH_URL=http://localhost:3000
IDENTIDADE_ISSUER=http://localhost:5001
IDENTIDADE_CLIENT_ID=aplicacao-web
IDENTIDADE_CLIENT_SECRET=segredo-aplicacao-web
NEXT_PUBLIC_URL_CATALOGO=http://localhost:5002
NEXT_PUBLIC_URL_CESTA=http://localhost:5003
NEXT_PUBLIC_URL_PEDIDOS=http://localhost:5004
```

### 3. Executar em desenvolvimento

```bash
npm run dev
```

Acesse: http://localhost:3000

### 4. Build para produção

```bash
npm run build
npm start
```

## Docker

### Build da imagem

```bash
docker build -t vendetudo-web .
```

### Executar container

```bash
docker run -p 3000:3000 \
  -e IDENTIDADE_ISSUER=http://identidade:5001 \
  -e NEXT_PUBLIC_URL_CATALOGO=http://catalogo:5002 \
  -e NEXT_PUBLIC_URL_CESTA=http://cesta:5003 \
  -e NEXT_PUBLIC_URL_PEDIDOS=http://pedidos:5004 \
  vendetudo-web
```

## Funcionalidades

### Catálogo
- Listagem paginada de produtos
- Filtros por tipo e marca
- Detalhes do produto
- Adicionar ao carrinho

### Cesta
- Visualizar itens no carrinho
- Alterar quantidades
- Remover itens
- Calcular total

### Checkout
- Formulário de endereço de entrega
- Confirmação do pedido

### Pedidos
- Histórico de pedidos
- Status do pedido
- Detalhes do pedido

### Usuário
- Visualizar perfil
- Autenticação via OpenID Connect

## Componentes shadcn/ui

Os componentes em `components/ui/` são gerenciados pelo shadcn/ui e **não devem ser editados diretamente**.

Para estender funcionalidade, crie wrappers ou novos componentes que usem esses primitivos.

## Convenções de Código

- **Server Components**: Default para todas as páginas
- **Client Components**: Apenas quando necessário (`"use client"`)
- **TypeScript**: Strict mode, sem `any`
- **Imports**: Use alias `@/` para paths absolutos
- **Estilização**: Tailwind CSS com design tokens (CSS variables)
- **Acessibilidade**: Componentes shadcn/ui já incluem ARIA

## Scripts

- `npm run dev` - Servidor de desenvolvimento
- `npm run build` - Build de produção
- `npm start` - Servidor de produção
- `npm run lint` - Linter ESLint

## Troubleshooting

### Erro de fetch ao listar produtos
- Verifique se o serviço de Catálogo está rodando
- Confirme a variável `NEXT_PUBLIC_URL_CATALOGO`

### Autenticação não funciona
- Verifique se o serviço de Identidade está rodando
- Confirme as variáveis `IDENTIDADE_*`
- Certifique-se de que o `AUTH_SECRET` está definido

### Build falha com erros TypeScript
- Execute `npm run lint` para ver erros
- Verifique imports e tipos

## Arquitetura

### Server vs Client Components

- **Server Components** (padrão):
  - Páginas de listagem
  - Detalhes de produtos
  - Histórico de pedidos
  - Melhor performance, SEO e segurança

- **Client Components** (quando necessário):
  - Formulários interativos
  - Controles de carrinho
  - Filtros e paginação
  - Uso de localStorage

### Camada de Serviços

Todas as chamadas à API são encapsuladas em `src/servicos/`:
- Facilita manutenção
- Centraliza lógica de fetch
- Type-safe com TypeScript

### Estado Local

Para simplificar, o carrinho usa `localStorage`:
- Funciona sem autenticação
- Sincroniza com API no checkout
- Produção deveria usar contexto + API

## Próximos Passos

- [ ] Implementar autenticação completa
- [ ] Substituir localStorage por estado global (Zustand)
- [ ] Adicionar testes (Vitest + Testing Library)
- [ ] Implementar validação de formulários com React Hook Form + Zod
- [ ] Melhorar tratamento de erros
- [ ] Adicionar loading states com Suspense
- [ ] Implementar retry logic nas chamadas de API
- [ ] Adicionar analytics e monitoramento

## Licença

Projeto educacional VendeTudo.
