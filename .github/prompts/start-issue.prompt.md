---
description: 'Use when starting any new work item, feature, fix, or investigation — when a developer says "start a new issue", "begin new work", "I want to work on X", "create a new requirement", "work on feature", "fix the issue". Confirms the primary branch, offers to use current branch or create a new one from develop, then starts the discussion phase. Required before writing any code.'
agent: 'analyst'
tools: ['terminal', 'editFiles']
model: 'claude-sonnet-4-5'
---
# Start Issue — Branch First, Then Define

Antes de qualquer código, configuramos o workspace correto.

**Descrição do issue**: ${input:issue-description:O que você está construindo? (ex: "adicionar rate limiting ao endpoint de login")}
**ID do Issue**: ${input:issue-id:ID do Issue (ex: ISSUE-042 ou número do GitHub #42)}

---

## Passo 1 — Confirmar Branch Primária

```bash
git branch -a
```

Verifique `.github/copilot-instructions.md` para linha como:
```
Primary branch: develop
```

**Se já registrado** → anote e vá para o Passo 2.

**Se não registrado**, pergunte ao desenvolvedor:
> "Qual é a branch primária do time? (geralmente: `develop` no GitFlow)"

Após resposta, adicione ao `## Conventions` do `copilot-instructions.md`:
```
Primary branch: develop
```

---

## Passo 2 — Verificar Branch Atual

```bash
git branch --show-current
```

Mostre a branch atual e pergunte:
> "Você está em `<branch-atual>`.
>
> **A) Permanecer em `<branch-atual>`** — continuar aqui
> **B) Criar nova branch a partir de `develop`** — recomendado para trabalho novo"

---

## Passo 3B — Criar Branch (se escolheu B)

```bash
git checkout develop
git pull origin develop
git checkout -b feature/${input:issue-id}-${input:issue-slug:slug curto (ex: login-rate-limiting)}
```

---

## Passo 4 — Verificar Baseline

```bash
# .NET
dotnet build && dotnet test

# Next.js
npm test
```

> **Se testes falharem:** Reporte as falhas. Pergunte: "Testes existentes estão falhando. Devemos corrigir antes de começar?"
> **NÃO prossiga** com novo trabalho em baseline vermelha sem aprovação explícita.

---

## Passo 5 — Criar Pasta work/

```powershell
$workDir = "work/${input:issue-id}-${input:issue-slug}"
New-Item -ItemType Directory -Force -Path $workDir
New-Item -ItemType File -Force -Path "$workDir/plan.md"
New-Item -ItemType File -Force -Path "$workDir/result.md"
```

Inicialize `plan.md` com:
```markdown
# ${input:issue-id}: ${input:issue-description}

## Fase 1: Requisitos
Status: Em andamento

### O que precisa ser feito
[Preencher com discussão]

### Critérios de Aceite
- [ ] 

### Fora do Escopo
- 
```

---

## Passo 6 — Oferecer Automação GitHub

Se for repositório GitHub:
```
🤖 Repositório GitHub detectado. Posso:
1. Criar um issue no GitHub para rastreamento
2. Vincular esta branch ao issue

Quer que eu automatize isso? (sim/não)
```

---

## Passo 7 — Iniciar Fase de Requisitos

Branch confirmada. Pasta work/ criada. Baseline verificada.

Pergunte: "Me conte mais sobre o que você quer construir com **${input:issue-description}**."

Documente os requisitos em `work/${input:issue-id}-${input:issue-slug}/plan.md`.
