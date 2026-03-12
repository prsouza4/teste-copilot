# 🤖 teste-copilot — Plataforma de Agentes de IA para Desenvolvimento

> Sistema completo de automação com **31 agentes especializados**, **6 skills**, **6 slash commands**, **hooks de sessão** e **workflows automáticos** — tudo coordenado pelo GitHub Copilot para .NET 10 + Next.js.

---

## 📋 Sobre o Projeto

Este repositório é uma **plataforma de engenharia assistida por IA** que cobre todo o ciclo de vida do software:

- **Análise e planejamento** — triagem automática de issues, geração de PRDs, decomposição de tarefas
- **Implementação** — agentes especializados em .NET 10 (Clean Architecture, CQRS, DDD), Next.js (App Router, shadcn/ui), containerização e mensageria
- **Qualidade e revisão** — quality gate em PRs com análise paralela de segurança, QA e arquitetura
- **Observabilidade** — instrumentação com OpenTelemetry, Serilog e health checks
- **Experiência do desenvolvedor** — slash commands, skills reutilizáveis, hooks de sessão e rastreamento de trabalho

Cada agente é definido em `.github/agents/` e pode ser **invocado pela CLI**, **acionado por GitHub Actions** automaticamente, ou **chamado via `#runSubagent`** no VS Code.

---

## 🗂️ Catálogo de Agentes

### 🧭 Orquestração e Estratégia

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Orquestrador** | `orchestrator.agent.md` | Coordenador central que roteia tarefas para os agentes certos, gerencia handoffs e sintetiza resultados. Use para tarefas multi-etapa que precisam de coordenação completa de ponta a ponta. |
| **Conselheiro Estratégico** | `high-level-advisor.agent.md` | Consultor honesto e direto que prioriza ruthlessly, desafia suposições e expõe pontos cegos. Use quando precisar de clareza e decisão, não de validação. |
| **Roadmap** | `roadmap.agent.md` | CEO do produto — define o que construir e por quê, com visão orientada a resultados. Cria epics, prioriza por valor de negócio usando RICE e KANO, e combate o desvio estratégico. |
| **Pensador Independente** | `independent-thinker.agent.md` | Analista contrário que desafia suposições com evidências e apresenta pontos de vista alternativos. Use como advogado do diabo quando precisar de crítica oposta. |

---

### 🔬 Análise e Planejamento

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Analista** | `analyst.agent.md` | Especialista em pesquisa que investiga causas raiz, levanta incógnitas e coleta evidências antes da implementação. Use para análise de impacto, descoberta de requisitos e validação de hipóteses. |
| **Arquiteto** | `architect.agent.md` | Autoridade técnica em design de sistemas. Cria ADRs, conduz revisões de design e garante que decisões sigam princípios de separação, extensibilidade e consistência. |
| **Planejador de Milestones** | `milestone-planner.agent.md` | Traduz epics do roadmap em pacotes de trabalho prontos para implementação, com milestones claros, dependências e critérios de aceitação. |
| **Decompositor de Tarefas** | `task-decomposer.agent.md` | Quebra PRDs e epics em itens de trabalho atômicos, estimáveis e com critérios claros de "pronto". Sequencia por dependências e agrupa em milestones. |
| **Gerador de Backlog** | `backlog-generator.agent.md` | Analisa proativamente o estado do projeto (issues abertas, PRs, saúde do código) e cria 3 a 5 tarefas acionáveis quando os agentes estão ociosos. |

---

### 🏗️ Implementação

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Implementador** | `implementer.agent.md` | Engenheiro focado em execução que implementa planos aprovados com código de qualidade de produção. Aplica SOLID, DRY, YAGNI e escreve testes junto com o código. |
| **Frontend** | `frontend.agent.md` | Especialista em frontend com React.js, Next.js (App Router), shadcn/ui e Tailwind CSS. Enforça TypeScript strict, acessibilidade, performance e boas práticas de desenvolvimento web moderno. |
| **Backend .NET** | `backend.agent.md` | Arquiteto e engenheiro sênior .NET com expertise em C#, ASP.NET Core, microserviços, CQRS, Event Sourcing, DDD, SOLID e Clean Architecture. Invocado com `--agent backend`. |

---

### ☁️ Microsserviços e Infraestrutura

> Estes três agentes trabalham em conjunto automático com o `backend`. Quando um novo microsserviço é criado, o orquestrador os aciona em sequência sem que você precise solicitar explicitamente.

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Infra** | `infra.agent.md` | Especialista em containerização e IaC. Cria Dockerfile multi-stage, docker-compose de desenvolvimento e manifestos Kubernetes (Deployment, Service, HPA, Ingress) para cada microsserviço .NET. Domina Terraform, Helm e deploy em Azure AKS, AWS EKS e GCP GKE. |
| **Messaging** | `messaging.agent.md` | Arquiteto de sistemas assíncronos e event-driven. Projeta contratos de eventos, implementa Outbox Pattern, Sagas (MassTransit), idempotência em consumers e retry/circuit breaker. Suporta RabbitMQ, Kafka e Azure Service Bus. |
| **Observability** | `observability.agent.md` | Engenheiro de observabilidade. Instrumenta microsserviços com OpenTelemetry (traces, métricas), Serilog (logs estruturados), health checks separados por `/ready` e `/live`, e configura a stack completa: Jaeger, Prometheus, Grafana e Seq. |

---

### ✅ Qualidade e Revisão

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Revisor de Código** | `code-reviewer.agent.md` | Revisa código para aderência a diretrizes do projeto, guias de estilo e boas práticas. Use proativamente antes de commits e pull requests. |
| **Crítico** | `critic.agent.md` | Revisor construtivo que testa planos sob pressão antes da implementação. Valida completude, identifica lacunas e bloqueia aprovação quando riscos não estão mitigados. |
| **QA** | `qa.agent.md` | Especialista em qualidade que verifica se implementações funcionam para usuários reais. Projeta estratégias de teste, valida cobertura e reporta resultados com evidências. |
| **Respondedor de PR** | `pr-comment-responder.agent.md` | Coordenador de revisões de PR que garante que todos os comentários dos revisores sejam endereçados sistematicamente. |
| **Analisador de Testes em PR** | `pr-test-analyzer.agent.md` | Analisa a qualidade e completude da cobertura de testes em pull requests. |
| **Caçador de Falhas Silenciosas** | `silent-failure-hunter.agent.md` | Identifica falhas silenciosas, tratamento inadequado de erros e comportamento de fallback inadequado. |
| **Simplificador de Código** | `code-simplifier.agent.md` | Simplifica código para clareza, consistência e manutenibilidade sem alterar funcionalidade. |
| **Analisador de Tipos** | `type-design-analyzer.agent.md` | Analisa o design de tipos no código, avaliando encapsulamento, invariantes e qualidade geral. |
| **Analisador de Comentários** | `comment-analyzer.agent.md` | Analisa comentários de código para precisão e manutenibilidade. Evita "comment rot". |

---

### 🔒 Segurança e DevOps

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Segurança** | `security.agent.md` | Especialista em segurança com mindset defense-first. Fluente em threat modeling, OWASP Top 10 e CWE. Detecta vulnerabilidades, audita dependências e mapeia superfícies de ataque. |
| **DevOps** | `devops.agent.md` | Especialista em CI/CD, automação de builds e workflows de deploy. Projeta GitHub Actions, configura sistemas de build e gerencia secrets. |

---

### 📚 Conhecimento e Memória

| Agente | Arquivo | Descrição |
|--------|---------|-----------|
| **Explicador** | `explainer.agent.md` | Especialista em documentação que escreve PRDs, explicações e especificações técnicas acessíveis, com critérios INVEST para user stories. |
| **Memória** | `memory.agent.md` | Gerencia continuidade entre sessões, recupera contexto relevante e mantém conhecimento institucional. |
| **Retrospectiva** | `retrospective.agent.md` | Extrai aprendizados via frameworks estruturados (Five Whys, análise de timeline). Transforma experiência em conhecimento institucional. |
| **Skillbook** | `skillbook.agent.md` | Gerencia habilidades aprendidas, transformando reflexões em atualizações atômicas. Previne duplicatas e mantém padrões limpos. |

---

## 💬 Slash Commands (Copilot Chat)

Comandos prontos acessíveis pelo ícone 📎 **Prompts** no Copilot Chat, ou digitando `#nome-do-prompt`:

| Comando | Quando usar |
|---------|-------------|
| `#start-issue` | Iniciar qualquer trabalho novo — configura branch, verifica baseline, cria `work/` |
| `#status` | Ver fase atual, tarefas restantes e próximo passo |
| `#debug` | Depuração sistemática em 4 fases (reproduzir → isolar → identificar → corrigir) |
| `#verify` | Checklist completo antes de abrir PR (testes, lint, requisitos) |
| `#finish-branch` | Finalizar trabalho — merge local, criar PR, manter ou descartar |
| `#summarize` | Salvar contexto da sessão para continuar depois ou fazer handoff |

> **Como invocar:** No Copilot Chat, clique em **📎 → Prompts** e selecione o comando desejado.

---

## 🧠 Skills

Skills são conhecimentos reutilizáveis que o Copilot aplica automaticamente quando o contexto é relevante:

| Skill | O que ensina |
|-------|--------------|
| `test-driven-development` | Ciclo Red-Green-Refactor para .NET xUnit + Next.js Vitest |
| `github-cli-workflow` | GitFlow completo — branch de `develop`, PRs, commits convencionais |
| `subagent-driven-development` | Como executar planos com múltiplos agentes em paralelo |
| `receiving-code-review` | Como tratar feedback de revisão de forma construtiva |
| `requesting-code-review` | Como preparar e solicitar revisões eficazes |
| `agent-activity-logger` | Como registrar atividade de agentes em `logs/copilot/` |

---

## 🔗 Hooks de Sessão

Automações que rodam em eventos da sessão do Copilot CLI:

| Hook | Evento | O que faz |
|------|--------|-----------|
| `session-auto-commit` | `Stop` | Faz commit automático de mudanças WIP ao fechar sessão (somente em branches `feature/` e `fix/`) |
| `session-logger` | `SessionStart` | Registra início de sessão em `logs/copilot/agent-activity.log` |
| `session-logger` | `Stop` | Registra fim de sessão e último commit |
| `session-logger` | `UserPromptSubmit` | Registra excerpt de cada prompt enviado (primeiros 120 chars) |

Logs ficam em `logs/copilot/agent-activity.log` (formato JSON por linha, gitignored).

---

## 🗂️ Rastreamento de Trabalho — `work/`

Cada issue tem sua própria pasta local (nunca commitada):

```
work/
└── ISSUE-042-login-rate-limiting/
    ├── plan.md    ← Requisitos, pesquisa, plano de tarefas
    └── result.md  ← Execução, testes, verificação, session notes
```

Criado automaticamente pelo `#start-issue`. Usado por `/status`, `/verify`, `/finish-branch` e `/summarize`.

---

## 🔧 Configuração de Ferramentas — `tool-sets.json`

Define quais ferramentas cada tipo de agente pode usar:

| Grupo | Ferramentas | Usado por |
|-------|-------------|-----------|
| `read-only` | search, codebase, problems | Exploração, análise, diagnóstico |
| `full-dev` | + terminal, editFiles, changes | Implementação, debugging, hooks |
| `review` | search, codebase, changes | Code review, QA, security |

---

## 🚀 Como Usar

### Pré-requisitos

- Conta GitHub com licença **GitHub Copilot**
- [GitHub Copilot CLI](https://docs.github.com/en/copilot/github-copilot-in-the-cli) instalado
- Secrets configurados no repositório:

---

### 💻 Executar Localmente

```bash
# 1. Clone o repositório
git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio

# 2. Invoque um agente diretamente pelo Copilot CLI
#    --allow-all libera criação/edição de arquivos e execução de comandos
#    -p executa em modo não-interativo (sai após concluir)

copilot --agent frontend --allow-all \
        -p "Crie um componente de Card de produto com imagem, título, preço e botão de adicionar ao carrinho usando shadcn/ui"

# 3. Use o orquestrador para tarefas complexas
copilot --agent orchestrator --allow-all \
        -p "Implemente autenticação JWT com login, logout e refresh token no backend .NET e tela de login no frontend Next.js"

# 4. Exemplo com agente backend .NET
copilot --agent backend --allow-all \
        -p "Desenhe o domínio de um sistema de pedidos usando DDD com CQRS, incluindo o Aggregate Order, Value Objects e Domain Events"

# 5. Exemplo com agente de segurança
copilot --agent security --allow-all \
        -p "Revise o endpoint de autenticação e identifique possíveis vulnerabilidades OWASP"
```

> **Dica:** O Copilot CLI carrega automaticamente o arquivo `.github/agents/<nome>.agent.md` ao usar `--agent <nome>`.
> Use `--allow-all` (ou `--yolo`) para permitir que o agente crie e edite arquivos sem confirmação manual.
> Use `-p` para modo não-interativo (ideal para scripts e automações) ou inicie sem `-p` para modo interativo.

---

### 🐛 Criar uma Issue para Triagem Automática por IA

Quando uma issue é criada neste repositório, o GitHub Actions dispara automaticamente os agentes **Analista** e **Roadmap** para categorizar, aplicar labels e alinhar ao milestone correto.

**Exemplo de issue bem formatada:**

```markdown
## Título
Implementar tela de listagem de produtos com filtros

## Descrição
Como usuário, quero visualizar uma lista de produtos com opção de filtrar
por categoria e faixa de preço, para encontrar rapidamente o que preciso.

## Critérios de Aceitação
- [ ] Exibir lista paginada de produtos (20 itens por página)
- [ ] Filtro por categoria (dropdown com shadcn/ui Select)
- [ ] Filtro por faixa de preço (slider com shadcn/ui Slider)
- [ ] Loading skeleton durante carregamento
- [ ] Mensagem de estado vazio quando não há resultados

## Contexto Técnico
- Frontend: Next.js + shadcn/ui
- API: endpoint GET /api/products já existente
- Design: seguir sistema de design atual
```

**O que acontece automaticamente após criar a issue:**

1. 🏷️ **Analista** categoriza e aplica labels (`enhancement`, `frontend`, `priority:P2`, etc.)
2. 🗺️ **Roadmap** alinha ao milestone mais adequado
3. 📝 Para issues complexas, o **Explicador** gera um PRD automaticamente
4. 💬 Um comentário de triagem é postado na issue com o resumo da análise

---

## 📁 Estrutura do Repositório

```
teste-copilot/
├── .github/
│   ├── agents/                     ← 31 agentes especializados (.agent.md)
│   ├── prompts/                    ← 18 templates de prompt (6 slash commands + quality gates + triage)
│   ├── skills/                     ← 6 skills reutilizáveis (TDD, GitFlow, code review, logging)
│   ├── instructions/               ← 5 instruções contextuais automáticas por tipo de arquivo
│   ├── hooks/
│   │   ├── session-auto-commit/    ← Auto-commit WIP ao fechar sessão (.sh + .ps1)
│   │   └── session-logger/         ← Log de sessão e prompts (.sh + .ps1)
│   ├── workflows/                  ← 4 pipelines GitHub Actions automáticos
│   ├── actions/                    ← 4 GitHub Actions compostas reutilizáveis
│   ├── scripts/                    ← 6 scripts Python para processamento de output dos agentes
│   ├── ISSUE_TEMPLATE/             ← 6 templates de issues (bug, feature, hotfix por contexto)
│   ├── copilot-instructions.md     ← Configuração mestre do Copilot
│   ├── copilot-code-review.md      ← Diretrizes de code review
│   ├── tool-sets.json              ← 3 grupos de ferramentas por tipo de agente
│   ├── labeler.yml                 ← Auto-labeling de PRs por contexto
│   └── PULL_REQUEST_TEMPLATE.md    ← Template padrão de PR
├── .vscode/
│   └── settings.json               ← Integração VS Code (prompt picker, instruction files)
├── work/                           ← Rastreamento local de issues (gitignored)
│   └── ISSUE-xxx-nome/
│       ├── plan.md
│       └── result.md
├── .gitignore                      ← Ignora logs/, work/, bin/, node_modules/, etc.
└── README.md
```

---

## 📁 Estrutura detalhada do `.github/`

Todo o sistema de agentes vive dentro da pasta `.github/`. Cada subdiretório tem um papel específico:

---

### 🤖 `.github/agents/`

**O que é:** Definições de cada agente especializado em formato Markdown.

**Para que serve:** Cada arquivo `.agent.md` é o "manual de trabalho" do agente — define sua persona, especialidade, regras de comportamento, ferramentas disponíveis, modelo de IA e padrões de output. O Copilot CLI carrega automaticamente o arquivo correto ao usar `--agent <nome>`.

```
analyst.agent.md         ← copilot --agent analyst ...
backend.agent.md  ← copilot --agent backend ...
frontend.agent.md        ← copilot --agent frontend ...
api-docs.agent.md        ← copilot --agent api-docs ...
```

---

### 💬 `.github/prompts/`

**O que é:** Templates de prompt reutilizáveis por tarefa específica.

**Para que serve:** Enquanto o `.agent.md` define *quem* o agente é, o prompt define *o que fazer* naquela execução. Os workflows passam o arquivo de prompt com o contexto da issue ou PR. Isso separa a identidade do agente da instrução da tarefa.

```
issue-triage-categorize.md   ← instrução para categorizar uma issue
issue-prd-generation.md      ← instrução para gerar um PRD
pr-quality-gate-security.md  ← instrução para revisar segurança de um PR
api-docs-generate.md         ← instrução para documentar endpoints
```

---

### 📋 `.github/instructions/`

**O que é:** Instruções contextuais aplicadas automaticamente pelo Copilot em arquivos específicos.

**Para que serve:** Usa a frontmatter `applyTo` para injetar regras automaticamente quando o Copilot edita certos tipos de arquivo. Ao editar qualquer arquivo de teste, as instruções de `testing.instructions.md` são aplicadas sem precisar pedir explicitamente.

```
testing.instructions.md        ← aplicado em arquivos de teste
security.instructions.md       ← aplicado em arquivos de auth/segurança
documentation.instructions.md  ← aplicado em arquivos .md
agent-prompts.instructions.md  ← aplicado em AGENTS.md e copilot-instructions.md
```

---

### ⚙️ `.github/workflows/`

**O que é:** Pipelines do GitHub Actions que disparam os agentes automaticamente.

**Para que serve:** Define *quando* cada agente é chamado. Cada workflow responde a um evento do GitHub (issue aberta, PR criado, label adicionada, arquivo alterado) e orquestra a chamada dos agentes.

```
ai-issue-triage.yml     ← issue aberta → chama analyst + roadmap
ai-pr-quality-gate.yml  ← PR aberto → chama security, qa, architect
api-docs.yml            ← .cs com endpoints alterado → chama api-docs
```

---

### 🔧 `.github/actions/`

**O que é:** GitHub Actions compostas e reutilizáveis chamadas pelos workflows.

**Para que serve:** Encapsula a lógica de invocar o Copilot CLI de forma padronizada — autenticação, retry, coleta de output e timeout. Os workflows chamam essas actions em vez de duplicar esse código em cada `.yml`.

```
ai-review/                   ← invoca copilot --agent ... -p ... e retorna output
check-agent-infrastructure/  ← verifica se o Copilot CLI está disponível
setup-code-env/              ← prepara ambiente (git, gh CLI, Python)
workflow-debounce/           ← evita execuções duplicadas em eventos rápidos
```

---

### 🐍 `.github/scripts/`

**O que é:** Scripts Python auxiliares chamados pelos workflows.

**Para que serve:** Processa o output dos agentes — parse de JSON, extração de labels, aplicação de milestones e postagem de comentários. A lógica de negócio fica em scripts testáveis, não embutida no YAML.

```
parse_triage_labels.py     ← extrai labels do output do agente analyst
parse_triage_roadmap.py    ← extrai milestone do output do agente roadmap
post_issue_comment.py      ← posta/atualiza comentário na issue ou PR
generate_quality_report.py ← gera relatório de qualidade de PRs
aggregate_quality_verdicts.py ← consolida veredictos dos agentes de qualidade
parse_feature_review.py    ← processa output de revisão de features
```

---

### 📝 `.github/ISSUE_TEMPLATE/`

Templates para criação de issues no GitHub — garante informações suficientes para o agente de triagem funcionar bem:

```
bug-backend.md       ← Bug em API/serviço .NET
bug-frontend.md      ← Bug em componente Next.js
feature-backend.md   ← Nova feature de API ou domínio
feature-frontend.md  ← Nova tela ou componente
feature-infra.md     ← Infra, pipelines, containers
hotfix.md            ← Correção urgente em produção
```

---

### 📄 Arquivos raiz do `.github/`

| Arquivo | Para que serve |
|---------|----------------|
| `copilot-instructions.md` | Configuração mestre — delegação de agentes, GitFlow, protocolo de sessão |
| `copilot-code-review.md` | Diretrizes de code review — máx. 20 comentários, alta confiança apenas |
| `tool-sets.json` | 3 grupos de ferramentas: `read-only`, `full-dev`, `review` |
| `labeler.yml` | Auto-labeling de PRs por arquivos alterados e conventional commits |
| `PULL_REQUEST_TEMPLATE.md` | Template padrão para descrição de PRs |

---

## 🔄 Como os Agentes Trabalham Juntos

### Fluxo de desenvolvimento (desenvolvedor local)

```
1. #start-issue "implementar rate limiting no login"
   ├─► Cria branch feature/ISSUE-042-login-rate-limiting a partir de develop
   ├─► Verifica baseline (dotnet test + npm test)
   └─► Cria work/ISSUE-042-login-rate-limiting/{plan.md, result.md}

2. Desenvolve com TDD
   ├─► Skill TDD: Red (teste falha) → Green (implementa) → Refactor
   └─► Skill GitHub CLI: commits convencionais, sync com origin

3. #verify → checklist de testes + lint + requisitos

4. #finish-branch → cria PR apontando para develop
```

### Fluxo automático para microsserviços

```
Você pede:
  "Crie o microsserviço de Pedidos com CQRS e DDD"

O orquestrador delega em sequência:
  ├─► backend      → Domain, CQRS handlers, API controllers (.NET 10)
  ├─► infra        → Dockerfile multi-stage, docker-compose, manifesto K8s
  ├─► messaging    → Contrato OrderPlaced, Outbox Pattern, Consumer (MassTransit)
  └─► observability → OpenTelemetry, Serilog, health checks /ready + /live
```

### Fluxo automático de Issue (GitHub Actions)

```
Issue criada no GitHub
    │
    ├─► ai-issue-triage.yml
    │       ├─► analyst    → categoriza, aplica labels (priority, area, type)
    │       ├─► roadmap    → alinha ao milestone correto
    │       └─► explainer  → gera PRD (se issue complexa)
    │
    └─► Comentário de triagem postado na issue automaticamente
```

### Fluxo automático de PR (GitHub Actions)

```
PR aberto → develop
    │
    ├─► ai-pr-quality-gate.yml (agentes rodam em paralelo)
    │       ├─► security   → detecta vulnerabilidades OWASP
    │       ├─► qa         → valida cobertura de testes
    │       └─► architect  → verifica aderência arquitetural
    │
    ├─► ai-pr-auto-assign.yml → auto-assign por contexto (backend/frontend/infra)
    └─► ai-pr-description.yml → gera descrição se PR title < 50 chars
```

### Fluxo de sessão (hooks automáticos)

```
SessionStart
    └─► log-session-start.ps1 → registra início em logs/copilot/agent-activity.log

UserPromptSubmit
    └─► log-prompt.ps1 → registra excerpt do prompt

Stop
    ├─► auto-commit.ps1   → commit WIP automático (somente feature/fix branches)
    └─► log-session-end.ps1 → registra fim e último commit
```

---

## 📊 Resumo de Capacidades

| Categoria | Quantidade | Detalhes |
|-----------|-----------|---------|
| Agentes especializados | **31** | Orquestração, análise, implementação, infra, QA, segurança, memória |
| Slash commands | **6** | start-issue, debug, verify, status, finish-branch, summarize |
| Skills reutilizáveis | **6** | TDD, GitFlow, subagent, code review (2), activity logger |
| Workflows automáticos | **4** | Issue triage, PR quality gate, PR auto-assign, PR description |
| GitHub Actions compostas | **4** | ai-review, check-infra, setup-env, debounce |
| Templates de issue | **6** | bug/feature para backend, frontend e infra + hotfix |
| Hooks de sessão | **2 sistemas** | auto-commit + logger (SessionStart/Stop/UserPromptSubmit) |
| Scripts Python | **6** | Parse, post, aggregate — lógica testável fora do YAML |
| Instruções contextuais | **5** | testing, security, docs, claude-skills, agent-prompts |

---

## 📄 Licença

Este projeto está sob a licença MIT.
