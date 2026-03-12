---
name: agent-activity-logger
description: "Background reference skill defining the structured JSON log format written to logs/copilot/agent-activity.log by each agent on completion. Defines all required fields and provides query examples. Loads when log format, activity log, or agent logging is mentioned."
---

# Agent Activity Log — Format Reference

Cada agente append um entry JSON em `logs/copilot/agent-activity.log` quando completa seu trabalho. Isso cria um audit trail completo do ciclo de vida de cada issue.

## Localização do Log

```
logs/copilot/agent-activity.log     ← JSON Lines format (um objeto JSON por linha)
logs/copilot/session.log            ← Eventos de sessão (start/end)
logs/copilot/prompts.log            ← Prompts submetidos
```

> Adicione `logs/` ao `.gitignore` — esses são arquivos de auditoria locais.

## Formato do Log Entry

```json
{
  "timestamp": "2026-03-12T11:00:00Z",
  "issueId": "ISSUE-042",
  "issueName": "login-rate-limiting",
  "phase": "execute",
  "agent": "backend",
  "developer": "seu-nome",
  "status": "complete",
  "summary": "Implementado rate limiting com sliding window no endpoint POST /auth/login",
  "decisions": [
    "Usado sliding window (não fixed) para evitar burst na borda da janela",
    "Rate limit por email, não por IP (evita problemas de IP compartilhado)"
  ],
  "outputFile": "work/ISSUE-042-rate-limiting/result.md",
  "nextPhase": "verify"
}
```

## Campos

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `timestamp` | ISO 8601 | ✅ | Quando a fase completou |
| `issueId` | string | ✅ | ex: `ISSUE-042` |
| `issueName` | string | ✅ | kebab-case do nome do issue |
| `phase` | string | ✅ | `discuss` \| `research` \| `plan` \| `execute` \| `verify` |
| `agent` | string | ✅ | Qual agente completou |
| `developer` | string | ✅ | Quem rodou a sessão |
| `status` | string | ✅ | `complete` \| `blocked` \| `partial` |
| `summary` | string | ✅ | Resumo de 1-2 frases |
| `decisions` | string[] | ✅ | Decisões chave tomadas |
| `outputFile` | string | ✅ | Caminho para o arquivo atualizado |
| `nextPhase` | string | ✅ | Qual fase vem a seguir |
| `blockers` | string[] | ❌ | Bloqueadores (se `status: blocked`) |
| `filesChanged` | string[] | ❌ | Para fase execute |
| `testResults` | object | ❌ | Para fases execute/verify |

## Campos da Fase Execute

```json
{
  "phase": "execute",
  "filesChanged": [
    "src/backend/Auth/src/API/Controllers/AuthController.cs (modified)",
    "src/backend/Auth/src/Application/Commands/LoginCommand.cs (created)"
  ],
  "commits": [
    "abc1234 - test: ISSUE-042 rate limit tests",
    "def5678 - feat: ISSUE-042 apply rate limiting"
  ]
}
```

## Como Ler o Log

```bash
# Toda atividade para um issue específico
grep "ISSUE-042" logs/copilot/agent-activity.log | jq .

# Todas as fases concluídas hoje
cat logs/copilot/agent-activity.log | jq 'select(.timestamp | startswith("2026-03-12"))'

# Todas as sessões bloqueadas
cat logs/copilot/agent-activity.log | jq 'select(.status == "blocked")'

# Quais issues estão em andamento?
cat logs/copilot/agent-activity.log | jq '{issueId, phase, status}'
```

## Como Escrever no Log (PowerShell)

```powershell
$entry = @{
    timestamp  = (Get-Date -Format "yyyy-MM-ddTHH:mm:ssZ")
    issueId    = "ISSUE-042"
    issueName  = "login-rate-limiting"
    phase      = "execute"
    agent      = "backend"
    developer  = $env:USERNAME
    status     = "complete"
    summary    = "Implementado rate limiting"
    decisions  = @("Decisão 1", "Decisão 2")
    outputFile = "work/ISSUE-042-rate-limiting/result.md"
    nextPhase  = "verify"
} | ConvertTo-Json -Compress

New-Item -Force -Path "logs/copilot" -ItemType Directory | Out-Null
Add-Content "logs/copilot/agent-activity.log" $entry
```
