---
description: 'Use when a developer needs to check their current progress on an issue — when they say "where am I?", "what phase am I on?", "what should I do next?", "show status", "check progress". Shows which phases are complete and what the next step should be.'
agent: 'analyst'
tools: ['search', 'codebase']
model: 'claude-haiku-4.5'
---
# Status — Verificar Progresso Atual

Mostra onde você está no workflow de 5 fases e o que fazer a seguir.

**Issue doc**: ${input:issue-doc:Caminho para o work doc (ex: work/ISSUE-042-nome/plan.md) ou deixe em branco para buscar}

---

## O que Este Comando Faz

1. **Se você forneceu o caminho**: Lê aquele issue doc específico
2. **Se deixou em branco**: Busca em `work/` pelo doc modificado mais recentemente

Então exibe:
- ✅ Quais fases estão completas
- 🔄 Qual fase está em progresso
- ⬜ Quais fases não foram iniciadas
- 🎯 Qual deve ser o seu **próximo passo**
- ⚠️ Quaisquer bloqueadores

---

## Formato do Relatório de Status

```
📊 Relatório de Status
─────────────────────────────────────

Issue: ISSUE-042
Título: Login Rate Limiting
Branch: feature/42-login-rate-limiting
Status: execute

─────────────────────────────────────

Fases:
  ✅ Fase 1: Requisitos      (confirmados)
  ✅ Fase 2: Pesquisa        (contexto coletado)
  ✅ Fase 3: Plano           (tarefas definidas)
  🔄 Fase 4: Implementação   (5/8 tarefas completas)
  ⬜ Fase 5: Verificação     (não iniciado)

─────────────────────────────────────

Tarefas Restantes:
  - [ ] Teste: admin bypass para rate limit
  - [ ] Implementar: verificação de role admin
  - [ ] Atualizar docs da API

─────────────────────────────────────

🎯 PRÓXIMO PASSO:
   Continue a implementação TDD das tarefas restantes
   Use: /execute ou abra work/ISSUE-042/plan.md
─────────────────────────────────────
```

---

## Se Nenhum Issue Doc For Encontrado

```
📊 Nenhum Issue Ativo Encontrado

Nenhum documento de issue encontrado em work/

🎯 PRÓXIMO PASSO:
   Inicie um novo issue com: /start-issue
```

---

## Verificando o Log de Atividade

Opcionalmente, verifique o histórico:

```bash
# Toda atividade para este issue
grep "ISSUE-042" logs/copilot/agent-activity.log | jq .

# O que está em andamento hoje
cat logs/copilot/agent-activity.log | jq 'select(.timestamp | startswith("2026-03-12"))'
```

---

## Casos de Uso

- **Início do dia**: Verifique em qual issue você estava trabalhando ontem
- **Após pausa**: Lembre-se rapidamente onde parou
- **Handoff para colega**: Compartilhe progresso atual
- **Standup**: Sumário rápido do progresso de ontem

---

## Comandos Relacionados

- `/start-issue` — Iniciar novo trabalho
- `/debug` — Depurar problema específico
- `/verify` — Verificar se está pronto para PR
- `/finish-branch` — Finalizar e criar PR/merge
- `/summarize` — Salvar contexto da sessão
