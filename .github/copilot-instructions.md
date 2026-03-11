# GitHub Copilot Instructions

> **IMPORTANT**: This file is intentionally minimal to reduce context window bloat. All detailed instructions are in AGENTS.md.

## Pre-Execution Announcement (OBRIGATÓRIO)

Antes de iniciar qualquer implementação, sempre anuncie:

1. **Qual agente será usado** e por quê
2. **O que será feito** em bullets
3. **Quais arquivos serão criados/modificados**

Formato obrigatório:

```
🤖 Agente selecionado: `backend`
📋 Motivo: issue contém label `backend` e descreve uma API REST

📝 Plano de execução:
- Criar microsserviço Orders.API com Clean Architecture
- Implementar Command PlaceOrderCommand + Handler
- Criar endpoint POST /api/orders
- Adicionar testes unitários

📁 Arquivos que serão criados:
- src/orders-api/Orders.API.csproj
- src/orders-api/Program.cs
- src/orders-api/Application/Commands/PlaceOrderCommand.cs
- ...

Iniciando implementação...
```

Isso vale para CLI local (escreva no terminal antes de agir) e para o Copilot Coding Agent (escreva como comentário na issue antes de abrir o PR).

---



Sempre seguir GitFlow. Regras obrigatórias:

- **Branch base para features:** `develop` — NUNCA criar feature diretamente de `main`
- **Nomenclatura de branches:** `feature/<numero-issue>-<descricao-curta>` (ex: `feature/42-api-usuarios`)
- **Todo PR de feature** aponta para `develop`, não para `main`
- **Hotfixes** apontam para `main` E `develop`
- **Se a branch `develop` não existir:** crie-a a partir de `main` antes de criar a feature
- **Se a branch de feature não existir:** crie-a automaticamente a partir de `develop`

```
main        ← produção
develop     ← integração (base de toda feature)
feature/X   ← implementação (criada a partir de develop)
hotfix/X    ← correção urgente (criada a partir de main)
```

---

## Agent Delegation for Complex Tasks

For tasks requiring multiple steps, specialized expertise, or extensive context, delegate using `#runSubagent` rather than handling everything inline:

| When to Delegate | Agent | Example Prompt |
|------------------|-------|----------------|
| Multi-step coordination | `orchestrator` | "Implement OAuth 2.0 with tests and docs" |
| Codebase exploration | `analyst` | "Investigate why cache invalidation fails" |
| Architecture decisions | `architect` | "Design the event sourcing pattern for orders" |
| Backend .NET implementation | `backend` | "Implement the UserService with CQRS and DDD" |
| Frontend UI implementation | `frontend` | "Implement the login page using shadcn/ui" |
| Implementation work | `implementer` | "Implement the UserService per approved plan" |
| Plan validation | `critic` | "Review the migration plan for gaps" |
| Security review | `security` | "Assess auth flow for vulnerabilities" |
| API documentation | `api-docs` | "Document the new endpoints added in this PR" |
| Containerization, Kubernetes, Docker, IaC, Terraform, deploy | `infra` | "Crie Dockerfile e manifesto K8s para o OrdersService" |
| Mensageria, eventos, Saga, Outbox, RabbitMQ, Kafka, Service Bus | `messaging` | "Implemente comunicação assíncrona entre Orders e Inventory via RabbitMQ" |
| Traces, métricas, logs, OpenTelemetry, Prometheus, Grafana, health checks | `observability` | "Instrumente o OrdersService com OpenTelemetry e Serilog" |

## Regras de Delegação Automática para Microsserviços

Quando o agente `backend` cria ou modifica um microsserviço, **chame automaticamente** os seguintes agentes na sequência:

1. **`infra`** — Criar Dockerfile, entrada no docker-compose e manifesto Kubernetes
2. **`messaging`** — Se o serviço publica ou consome eventos, definir contratos e implementar Outbox/Consumer
3. **`observability`** — Adicionar instrumentação OpenTelemetry, Serilog, health checks e métricas

Não espere o usuário pedir. Se houver contexto de novo microsserviço, acione os três.

**Exemplo de delegação encadeada:**
```text
#runSubagent backend "Crie o serviço de Pedidos com CQRS e DDD"
  → após criar o serviço:
#runSubagent infra "Containerize o OrdersService e crie manifesto K8s"
#runSubagent messaging "Implemente publicação do evento OrderPlaced com Outbox Pattern"
#runSubagent observability "Instrumente o OrdersService com OpenTelemetry e health checks"
```

**Why delegate:**

- Manages context window efficiently (agents start fresh)
- Provides specialized system prompts and constraints
- Returns focused results you can synthesize

**Delegation pattern:**

```text
#runSubagent orchestrator "Help me implement feature X end-to-end"
```

**Keep inline:** Simple, single-file edits or quick lookups that don't require specialized reasoning.

## Primary Reference

**Read AGENTS.md FIRST** for complete instructions:

- Session protocol (blocking gates)
- Agent catalog and workflows
- Directory structure
- GitHub workflow requirements
- Skill system
- Memory management
- Quality gates

**Path**: `../AGENTS.md` (repository root)

## Serena MCP Initialization (BLOCKING)

If Serena MCP tools are available, you MUST call FIRST:

1. `serena/activate_project` (with project path)
2. `serena/initial_instructions`

**Check for**: Tools prefixed with `serena/` or `mcp__serena__`

**If unavailable**: Proceed without Serena, but reference AGENTS.md for full context

## Critical Constraints (Quick Reference)

> **Full Details**: `../.agents/governance/PROJECT-CONSTRAINTS.md`

| Constraint | Source |
|------------|--------|
| Python first (.py preferred, PowerShell grandfathered) | ADR-042 |
| No raw gh when skill exists | usage-mandatory |
| No logic in workflow YAML | ADR-006 |
| Verify branch before git operations | SESSION-PROTOCOL |
| HANDOFF.md is read-only | ADR-014 |

## Session Protocol (Quick Reference)

> **Full Details**: `../.agents/SESSION-PROTOCOL.md`

**Session Start:**

1. Initialize Serena (if available)
2. Read HANDOFF.md (read-only dashboard)
3. Create session log: `.agents/sessions/YYYY-MM-DD-session-NN.json`
4. Verify branch: `git branch --show-current`

**Session End:**

1. Complete session log
2. Update Serena memory (if available)
3. Run scoped markdownlint on changed files (ADR-043, see SESSION-PROTOCOL.md Phase 2)
4. Commit all changes
5. Run `python3 scripts/validate_session_json.py [log]`

## API Contract Reference (IMPORTANT for frontend agent)

Before implementing any UI that consumes backend APIs, **always check `docs/api/` first**:

```
docs/api/
├── README.md              ← index of all documented endpoints
├── users/
│   ├── post-create-user.md
│   └── get-list-users.md
└── orders/
    └── post-create-order.md
```

Each file contains: route, authentication, request/response schema, validation rules, and ready-to-use `fetch` + TypeScript examples.

**Rule:** If `docs/api/<resource>/<method>-<action>.md` exists → use it as the source of truth. Do not read the .NET source code directly.

**If the doc does not exist yet:** call `#runSubagent api-docs` to generate it before starting the UI implementation.

## Key Documents

1. **AGENTS.md** - Primary reference (read first)
2. `docs/api/README.md` - API contract index (frontend agent reads this first)
3. `.agents/SESSION-PROTOCOL.md` - Session requirements
4. `.agents/HANDOFF.md` - Project dashboard (read-only)
5. `.agents/governance/PROJECT-CONSTRAINTS.md` - Hard constraints
6. `.agents/AGENT-SYSTEM.md` - Full agent details

---

**For complete documentation, workflows, examples, and best practices, see [AGENTS.md](../AGENTS.md).**
