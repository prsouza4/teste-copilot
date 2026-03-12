---
name: requesting-code-review
description: "Use when completing a task, implementing a major feature, or before merging — when a developer says 'review my code', 'check this before I merge', 'is this ready?', or after completing any task in a subagent-driven workflow. Dispatches a code-reviewer subagent with structured input. Mandatory after each task in subagent-driven development and before any merge to main/develop. Auto-loads when review, PR, merge, or code quality is mentioned."
---

# Requesting Code Review

Despache o agente `code-reviewer` para capturar problemas antes que se propaguem.

**Core principle:** Revise cedo, revise com frequência.

## Quando Solicitar Revisão

**Obrigatório:**
- Após cada tarefa em subagent-driven development
- Após concluir feature principal
- Antes de merge para `develop` ou `main`

**Opcional mas valioso:**
- Quando travado (perspectiva nova)
- Antes de refactoring (check de baseline)
- Após corrigir bug complexo

## Como Solicitar

### 1. Obtenha os SHAs do git:
```bash
BASE_SHA=$(git rev-parse HEAD~1)  # ou origin/develop
HEAD_SHA=$(git rev-parse HEAD)
```

### 2. Despache o agente `code-reviewer`:

```
@code-reviewer Revise as mudanças de [BASE_SHA] até [HEAD_SHA].

O que foi implementado: [Descreva o que foi construído]
Requisitos/Plano: [O que deveria fazer]
SHAs: [base]..[head]
Resumo: [Breve sumário]
```

### 3. Aja no feedback:
- **Critical**: Corrija imediatamente, não prossiga
- **Important**: Corrija antes de continuar
- **Minor**: Note para depois

## Severidades

| Nível | Ação |
|-------|------|
| 🔴 Critical | Para tudo, corrija antes de qualquer outra coisa |
| 🟡 Important | Corrija antes de prosseguir para próxima tarefa |
| 🔵 Minor | Note, corrija quando conveniente |

## Integração com Workflows

**Subagent-Driven Development:**
- Revise após CADA tarefa
- Capture problemas antes que se propaguem
- Corrija antes de prosseguir

**Desenvolvimento ad-hoc:**
- Revise antes do merge
- Revise quando travado

## Red Flags

**Nunca:**
- Pule a revisão porque "é simples"
- Ignore problemas Critical
- Prossiga com problemas Important não corrigidos

**Se revisor estiver errado:**
- Conteste com raciocínio técnico (veja skill `receiving-code-review`)
- Mostre código/testes que provam que funciona
