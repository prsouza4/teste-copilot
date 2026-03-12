---
name: subagent-driven-development
description: "Use when executing an implementation plan with 2+ mostly-independent tasks in the current session — when a developer says 'execute this plan', 'implement these tasks', 'work through the plan', or when a written plan exists and coding is starting. Dispatches a fresh subagent per task to prevent context pollution, then runs two mandatory review stages per task: spec compliance (did code match requirements?) then code quality (is it well-written?). Distinct from parallel execution: subagent-driven-development stays in the same session. Auto-loads when plan execution, task implementation, or sequential development work is mentioned."
---

# Subagent-Driven Development

Execute plan by dispatching fresh subagent per task, with two-stage review after each: spec compliance review first, then code quality review.

**Core principle:** Fresh subagent per task + two-stage review (spec then quality) = high quality, fast iteration

## When to Use

- Você tem um plano de implementação
- As tarefas são majoritariamente independentes
- Quer manter tudo na mesma sessão (sem troca de contexto)

## The Process

### Por tarefa:
1. Despache subagent implementador com o texto completo da tarefa
2. Subagent implementa, testa, commita e faz self-review
3. Despache subagent de revisão de spec — confirma que o código atende aos requisitos
4. Se não atender: implementador corrige → revisor revisa novamente
5. Despache subagent de revisão de qualidade de código
6. Se não aprovar: implementador corrige → revisor revisa novamente
7. Marque a tarefa como concluída

### Após todas as tarefas:
- Despache revisor de código final para toda a implementação
- Use `/finish-branch` para finalizar

## Prompt Templates

- **Implementador**: "Implemente a tarefa [X] do plano. Contexto: [texto completo]. Use TDD. Commite ao finalizar."
- **Revisor de spec**: "Revise se o código implementado atende EXATAMENTE aos requisitos da tarefa [X]. Liste o que está em conformidade e o que está faltando."
- **Revisor de qualidade**: "Revise a qualidade do código commitado nos SHAs [base..head]. Avalie: legibilidade, testabilidade, acoplamento, SOLID."

## Regras

**NUNCA:**
- Pule as revisões (spec OU qualidade)
- Despache múltiplos subagents de implementação em paralelo (conflitos)
- Faça o subagent ler o arquivo de plano (forneça o texto completo)
- Aceite "mais ou menos correto" na revisão de spec
- Comece revisão de qualidade antes da spec estar ✅

**SE subagent fizer perguntas:**
- Responda clara e completamente antes de deixá-lo prosseguir

**SE revisor encontrar problemas:**
- Implementador (mesmo subagent) corrige
- Revisor revisa novamente
- Repita até aprovação

## Integração com Agentes deste Projeto

| Papel | Agente |
|-------|--------|
| Implementador backend | `backend` |
| Implementador frontend | `frontend` |
| Revisor de código | `code-reviewer` |
| Revisor de arquitetura | `architect` |
| QA/verificação | `qa` |
| Orquestrador | `orchestrator` |
